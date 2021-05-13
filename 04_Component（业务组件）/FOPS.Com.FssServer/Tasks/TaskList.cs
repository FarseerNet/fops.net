using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.Tasks.Dal;
using FS.Extends;

namespace FOPS.Com.FssServer.Tasks
{
    /// <summary>
    /// 任务列表
    /// </summary>
    public class TaskList : ITaskList
    {
        public ITaskAgent TaskAgent { get; set; }

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        public Task<List<TaskVO>> ToListAsync() => TaskAgent.ToListAsync().MapAsync<TaskVO, TaskPO>();

        /// <summary>
        /// 获取指定任务组执行成功的任务列表
        /// </summary>
        public Task<List<TaskVO>> ToSuccessListAsync(int groupId, int top) => TaskAgent.ToSuccessListAsync(groupId, top).MapAsync<TaskVO, TaskPO>();
        
        /// <summary>
        /// 清除成功的任务记录（1天前）
        /// </summary>
        public Task ClearSuccessAsync(int groupId, int taskId) => TaskAgent.ClearSuccessAsync(groupId, taskId);
    }
}