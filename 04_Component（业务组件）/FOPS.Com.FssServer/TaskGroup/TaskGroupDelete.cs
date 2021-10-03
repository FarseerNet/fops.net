using System.Threading.Tasks;
using FOPS.Abstract.Fss.Server;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FOPS.Infrastructure.Repository;

namespace FOPS.Com.FssServer.TaskGroup
{
    public class TaskGroupDelete : ITaskGroupDelete
    {
        public ITaskGroupUpdate TaskGroupUpdate { get; set; }
        public ITaskGroupInfo   TaskGroupInfo   { get; set; }
        public ITasksDelete     TasksDelete     { get; set; }
        public TaskGroupAgent   TaskGroupAgent  { get; set; }

        /// <summary>
        /// 删除任务组
        /// </summary>
        public async Task DeleteAsync(int taskGroupId)
        {
            var info = await TaskGroupInfo.ToInfoAsync(taskGroupId);
            // 如果任务组是开启状态，则需要先暂定任务组
            if (info.IsEnable)
            {
                info.IsEnable = false;
                await TaskGroupUpdate.SaveAsync(info);
            }
            
            // 删除当前任务组下的所有任务
            await TasksDelete.DeleteAsync(taskGroupId);
            await TaskGroupAgent.DeleteAsync(taskGroupId);
            await CacheKeys.TaskForGroupClear(taskGroupId);
            await CacheKeys.TaskGroupClear(taskGroupId);
        }
    }
}