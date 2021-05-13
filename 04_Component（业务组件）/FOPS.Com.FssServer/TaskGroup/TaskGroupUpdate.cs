using System;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Enum;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FOPS.Com.FssServer.Tasks.Dal;
using FS.Cache.Redis;
using FS.DI;
using FS.Extends;
using Newtonsoft.Json;

namespace FOPS.Com.FssServer.TaskGroup
{
    public class TaskGroupUpdate : ITaskGroupUpdate
    {
        public ITaskGroupCache TaskGroupCache { get; set; }
        public ITaskGroupAgent TaskGroupAgent { get; set; }
        public IIocManager     IocManager     { get; set; }

        /// <summary>
        /// 更新TaskGroup
        /// </summary>
        public Task UpdateAsync(TaskGroupVO taskGroup) => TaskGroupCache.SaveAsync(taskGroup.Id, taskGroup);

        /// <summary>
        /// 保存TaskGroup
        /// </summary>
        public async Task SaveAsync(TaskGroupVO taskGroup)
        {
            await UpdateAsync(taskGroup);
            var taskGroupPO = taskGroup.Map<TaskGroupPO>();
            taskGroupPO.Cron       = null;
            taskGroupPO.IntervalMs = null;
            taskGroupPO.IsEnable   = null;
            await TaskGroupAgent.UpdateAsync(taskGroup.Id, taskGroupPO);
        }
    }
}