using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Enum;
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
        public Task<List<TaskVO>> ToTopListAsync(int top) => TaskAgent.ToTopListAsync(top).MapAsync<TaskVO, TaskPO>();

        /// <summary>
        /// 获取指定任务组的任务列表
        /// </summary>
        public Task<List<TaskVO>> ToListAsync(int groupId, int pageSize, int pageIndex, out int totalCount)
        {
            return FssContext.Data.Task.Where(o => o.TaskGroupId == groupId)
                .Select(o => new { o.Id, o.Caption, o.Progress, o.Status, o.StartAt, o.CreateAt, o.ClientIp, o.RunSpeed, o.RunAt })
                .Desc(o => o.CreateAt).ToListAsync(pageSize, pageIndex, out totalCount).MapAsync<TaskVO, TaskPO>();
        }

        /// <summary>
        /// 获取失败的任务数量
        /// </summary>
        public Task<List<TaskVO>> ToFailListAsync(int pageSize, int pageIndex, out int totalCount)
        {
            return FssContext.Data.Task.Where(o => o.Status == EumTaskType.Fail)
                .Select(o => new { o.Id, o.Caption, o.Progress, o.Status, o.StartAt, o.CreateAt, o.ClientIp, o.RunSpeed, o.RunAt })
                .Desc(o => o.CreateAt).ToListAsync(pageSize, pageIndex, out totalCount).MapAsync<TaskVO, TaskPO>();
        }

        /// <summary>
        /// 获取未执行的任务列表
        /// </summary>
        public Task<List<TaskVO>> ToUnRunListAsync(int pageSize, int pageIndex, out int totalCount)
        {
            var now = DateTime.Now.AddMilliseconds(-500);
            return FssContext.Data.Task.Where(o => o.StartAt < now && o.Status == EumTaskType.None)
                .Select(o => new { o.Id, o.TaskGroupId, o.JobName, o.Caption, o.Progress, o.Status, o.StartAt, o.CreateAt, o.ClientIp, o.RunSpeed, o.RunAt })
                .Asc(o => o.StartAt).ToListAsync(pageSize, pageIndex, out totalCount).MapAsync<TaskVO, TaskPO>();
        }
    }
}