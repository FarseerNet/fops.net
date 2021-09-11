using System;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.Tasks.Dal;
using FS.Cache;
using FS.Cache.Redis;
using FS.DI;
using FS.Extends;
using FS.Utils.Component;

namespace FOPS.Com.FssServer.Tasks
{
    public class TaskUpdate : ITaskUpdate
    {
        public  ITaskInfo          TaskInfo          { get; set; }
        public  ITaskAgent         TaskAgent         { get; set; }
        public  ITaskGroupUpdate   TaskGroupUpdate   { get; set; }
        public  IIocManager        IocManager        { get; set; }
        public  ITaskAdd           TaskAdd           { get; set; }
        private IRedisCacheManager RedisCacheManager => IocManager.Resolve<IRedisCacheManager>("fss_redis");


        /// <summary>
        /// 移除缓存
        /// </summary>
        public async Task ClearCacheAsync()
        {
            await RedisCacheManager.Db.KeyDeleteAsync(TaskCache.Key);
            await TaskInfo.ToGroupListAsync();
        }
        
        /// <summary>
        /// 任务组修改时，需要同步JobName
        /// </summary>
        public async Task UpdateJobName(int taskId, string jobName)
        {
            await FssContext.Data.Task.Where(o => o.Id == taskId).UpdateAsync(new TaskPO() { JobName = jobName });
            var task = await TaskInfo.ToInfoByDbAsync(taskId);
            if (task == null) return;
            await RedisCacheManager.CacheManager.SaveAsync(TaskCache.Key, task, task.TaskGroupId, new CacheOption());
        }

        /// <summary>
        /// 更新Task（如果状态是成功、失败、重新调度，则应该调Save）
        /// </summary>
        public async Task UpdateAsync(TaskVO task)
        {
            await RedisCacheManager.CacheManager.SaveAsync(TaskCache.Key, task, task.TaskGroupId, new CacheOption());
        }
        
        /// <summary>
        /// 保存Task
        /// </summary>
        public async Task SaveAsync(TaskVO task)
        {
            await UpdateAsync(task);
            await TaskAgent.UpdateAsync(task.Id, task.Map<TaskPO>());
        }

        /// <summary>
        /// 保存Task（taskGroup必须是最新的）这里与FSS的逻辑不一致。因为FSS需要发消息
        /// </summary>
        public async Task SaveFinishAsync(TaskVO task, TaskGroupVO taskGroup)
        {
            // 说明上一次任务，没有设置下一次的时间（动态设置）
            // 本次的时间策略晚，则通过时间策略计算出来
            if (DateTime.Now > taskGroup.NextAt)
            {
                var cron = new Cron();
                // 时间间隔器
                if (taskGroup.IntervalMs > 0) taskGroup.NextAt = DateTime.Now.AddMilliseconds(taskGroup.IntervalMs);
                else if (!string.IsNullOrWhiteSpace(taskGroup.Cron) && cron.Parse(taskGroup.Cron))
                {
                    taskGroup.NextAt = cron.GetNext(DateTime.Now);
                }
                else // 没有找到设置下一次时间的设置，则默认30S执行一次
                {
                    taskGroup.NextAt = DateTime.Now.AddSeconds(30);
                }
            }

            await TaskGroupUpdate.UpdateAsync(taskGroup);

            if (taskGroup.IsEnable)
            {
                // 完成后，立即生成一个新的任务
                await TaskAdd.GetOrCreateAsync(taskGroup);
            }

            await SaveAsync(task);
        }
    }
}