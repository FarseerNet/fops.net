using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Com.FssServer.Abstract;

namespace FOPS.Com.FssServer.TaskGroup.Dal
{
    /// <summary>
    /// 任务组数据库层
    /// </summary>
    public class TaskGroupAgent : ITaskGroupAgent
    {
        /// <summary>
        /// 获取所有任务组列表
        /// </summary>
        public Task<List<TaskGroupPO>> ToListAsync() => FssContext.Data.TaskGroup.ToListAsync();

        /// <summary>
        /// 获取任务组信息
        /// </summary>
        public Task<TaskGroupPO> ToEntityAsync(int id) => FssContext.Data.TaskGroup.Where(o => o.Id == id).ToEntityAsync();

        /// <summary>
        /// 更新任务组信息
        /// </summary>
        public Task UpdateAsync(int id, TaskGroupPO taskGroup) => FssContext.Data.TaskGroup.Where(o => o.Id == id).UpdateAsync(taskGroup);

        /// <summary>
        /// 添加任务组
        /// </summary>
        public Task AddAsync(TaskGroupPO po) => FssContext.Data.TaskGroup.InsertAsync(po, true);

    }
}