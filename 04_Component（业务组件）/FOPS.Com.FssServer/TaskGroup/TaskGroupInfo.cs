using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FS.Extends;

namespace FOPS.Com.FssServer.TaskGroup
{
    public class TaskGroupInfo : ITaskGroupInfo
    {
        public TaskGroupAgent TaskGroupAgent { get; set; }
        public TaskGroupCache TaskGroupCache { get; set; }

        /// <summary>
        /// 获取任务信息
        /// </summary>
        public Task<TaskGroupVO> ToInfoAsync(int id) => TaskGroupCache.ToEntityAsync(id);

        /// <summary>
        /// 从数据库中取出并保存
        /// </summary>
        public async Task<TaskGroupVO> ToInfoByDbAsync(int id)
        {
            var entity = await TaskGroupAgent.ToEntityAsync(id).MapAsync<TaskGroupVO, TaskGroupPO>();
            await TaskGroupCache.SaveAsync(id, entity);
            return entity;
        }
    }
}