using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FS.Extends;

namespace FOPS.Com.FssServer.TaskGroup
{
    public class TaskGroupAdd : ITaskGroupAdd
    {
        public ITaskGroupAgent TaskGroupAgent { get; set; }
        public ITaskGroupInfo  TaskGroupInfo { get; set; }
        
        /// <summary>
        /// 添加任务信息
        /// </summary>
        public async Task ToInfoAsync(TaskGroupVO vo)
        {
            var po = vo.Map<TaskGroupPO>();
            // 添加到数据库
            await TaskGroupAgent.AddAsync(po);
            
            // 从数据库中读出刚添加的任务组，并保存到缓存
            await TaskGroupInfo.ToInfoByDbAsync(po.Id.GetValueOrDefault());
        }
    }
}