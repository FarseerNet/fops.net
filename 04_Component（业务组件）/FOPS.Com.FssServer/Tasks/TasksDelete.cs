using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.FssServer.Tasks.Dal;

namespace FOPS.Com.FssServer.Tasks
{
    public class TasksDelete : ITasksDelete
    {
        public TaskAgent TaskAgent { get; set; }

        /// <summary>
        /// 删除任务组
        /// </summary>
        public Task DeleteAsync(int taskGroupId) => TaskAgent.DeleteAsync(taskGroupId);
    }
}