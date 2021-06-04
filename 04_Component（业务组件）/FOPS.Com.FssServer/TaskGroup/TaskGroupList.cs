using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Enum;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FS.Cache.Redis;
using FS.DI;
using FS.Extends;
using StackExchange.Redis;

namespace FOPS.Com.FssServer.TaskGroup
{
    /// <summary>
    /// 任务列表
    /// </summary>
    public class TaskGroupList : ITaskGroupList
    {
        public ITaskGroupAgent TaskGroupAgent { get; set; }
        public IIocManager     IocManager     { get; set; }

        private IRedisCacheManager RedisCacheManager => IocManager.Resolve<IRedisCacheManager>("fss_redis");

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        public async Task<List<TaskGroupVO>> ToListAndSaveAsync()
        {
            var taskGroupVos = await TaskGroupAgent.ToListAsync().MapAsync<TaskGroupVO, TaskGroupPO>();
            await RedisCacheManager.CacheManager.SaveAsync(TaskGroupCache.Key, taskGroupVos, o => o.Id);
            return taskGroupVos;
        }

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        public Task<List<TaskGroupVO>> ToListAsync()
        {
            return RedisCacheManager.CacheManager.GetListAsync(TaskGroupCache.Key,
                _ => TaskGroupAgent.ToListAsync().MapAsync<TaskGroupVO, TaskGroupPO>()
                , o => o.Id);
        }

        /// <summary>
        /// 获取任务组数量
        /// </summary>
        public Task<long> Count()
        {
            return RedisCacheManager.Db.HashLengthAsync(TaskGroupCache.Key);
        }

        /// <summary>
        /// 删除整个缓存
        /// </summary>
        public Task ClearAsync() => RedisCacheManager.CacheManager.RemoveAsync(TaskGroupCache.Key);

        /// <summary>
        /// 获取未执行的任务列表
        /// </summary>
        public async Task<int> ToUnRunCountAsync()
        {
            var now = DateTime.Now.AddMilliseconds(-500);
            var lst = await ToListAndSaveAsync();
            return lst.Count(o => o.NextAt < now && o.IsEnable == true);
        }
    }
}