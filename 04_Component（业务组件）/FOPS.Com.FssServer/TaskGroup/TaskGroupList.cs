using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FOPS.Infrastructure.Repository;
using FS.Cache;
using FS.DI;
using FS.Extends;
using Microsoft.Extensions.Logging;

namespace FOPS.Com.FssServer.TaskGroup
{
    /// <summary>
    /// 任务列表
    /// </summary>
    public class TaskGroupList : ITaskGroupList
    {
        public TaskGroupAgent TaskGroupAgent { get; set; }
        public TaskGroupCache TaskGroupCache { get; set; }

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        public async Task<List<TaskGroupVO>> ToListAndSaveAsync()
        {
            var taskGroupVos = await TaskGroupAgent.ToListAsync().MapAsync<TaskGroupVO, TaskGroupPO>();
            await TaskGroupCache.SaveAsync(taskGroupVos);
            return taskGroupVos;
        }

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        public Task<List<TaskGroupVO>> ToListInCacheAsync(EumCacheStoreType cacheStoreType = EumCacheStoreType.Redis) => TaskGroupCache.ToListAsync(cacheStoreType);

        /// <summary>
        /// 获取任务组数量
        /// </summary>
        public Task<long> Count()
        {
            var key = CacheKeys.TaskGroupKey(EumCacheStoreType.Redis);
            return RedisContext.Instance.CacheManager.GetCountAsync(key);
        }

        /// <summary>
        /// 获取未执行的任务列表
        /// </summary>
        public async Task<int> ToUnRunCountAsync()
        {
            try
            {
                var now = DateTime.Now.AddMilliseconds(-500);
                var lst = await ToListAndSaveAsync();
                return lst.Count(o => o.NextAt < now && o.IsEnable);
            }
            catch (Exception e)
            {
                IocManager.Instance.Logger<TaskGroupList>().LogError(e, e.Message);
                return 0;
            }
        }
    }
}