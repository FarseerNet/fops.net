using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Enum;
using FOPS.Com.FssServer.Abstract;

namespace FOPS.Com.FssServer.Tasks.Dal
{
    /// <summary>
    /// 任务数据库层
    /// </summary>
    public class TaskAgent : ITaskAgent
    {
        /// <summary>
        /// 获取所有任务列表
        /// </summary>
        public Task<List<TaskPO>> ToListAsync() => FssContext.Data.Task.ToListAsync();

        /// <summary>
        /// 获取指定任务组执行成功的任务列表
        /// </summary>
        public Task<List<TaskPO>> ToSuccessListAsync(int groupId, int top) => FssContext.Data.Task.Where(o => o.TaskGroupId == groupId && o.Status == EumTaskType.Success).ToListAsync(top);

        /// <summary>
        /// 清除成功的任务记录（1天前）
        /// </summary>
        public Task ClearSuccessAsync(int groupId, int taskId) => FssContext.Data.Task.Where(o => o.TaskGroupId == groupId && o.Status == EumTaskType.Success && o.CreateAt < DateTime.Now.AddDays(-1) && o.Id < taskId).DeleteAsync();

        /// <summary>
        /// 获取任务信息
        /// </summary>
        public Task<TaskPO> ToEntityAsync(int id) => FssContext.Data.Task.Where(o => o.Id == id).ToEntityAsync();

        /// <summary>
        /// 更新任务信息
        /// </summary>
        public Task UpdateAsync(int id, TaskPO task) => FssContext.Data.Task.Where(o => o.Id == id).UpdateAsync(task);

        /// <summary>
        /// 添加任务信息
        /// </summary>
        public Task AddAsync(TaskPO task) => FssContext.Data.Task.InsertAsync(task, true);

        /// <summary>
        /// 获取未执行的任务信息
        /// </summary>
        public Task<TaskPO> ToUnExecutedTaskAsync(int groupId) => FssContext.Data.Task.Where(o => o.TaskGroupId == groupId && (o.Status == EumTaskType.None || o.Status == EumTaskType.Scheduler)).ToEntityAsync();

        /// <summary>
        /// 取前100条的运行速度
        /// </summary>
        public Task<List<int>> ToSpeedListAsync(int groupId) => FssContext.Data.Task.Where(o => o.TaskGroupId == groupId && o.Status == EumTaskType.Success).Desc(o => o.Id).ToSelectListAsync(100, o => o.RunSpeed.GetValueOrDefault());

        /// <summary>
        /// 今日执行失败数量
        /// </summary>
        public Task<int> TodayFailCountAsync() => FssContext.Data.Task.Where(o => o.CreateAt >= DateTime.Now.Date).CountAsync();
    }
}