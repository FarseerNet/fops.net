using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.Cache;

namespace FOPS.Infrastructure.Repository
{
    public class CacheKeys
    {
        /// <summary> 任务组缓存 </summary>
        public static CacheKey<TaskGroupVO, int> TaskGroupKey(EumCacheStoreType cacheStoreType) => new($"FSS_TaskGroup", o => o.Id, cacheStoreType);
        public static Task TaskGroupClear(int taskGroupId) => RedisContext.Instance.CacheManager.RemoveAsync(TaskGroupKey(EumCacheStoreType.MemoryAndRedis), taskGroupId);


        /// <summary> 任务缓存 </summary>
        public static CacheKey<TaskVO, int> TaskForGroupKey => new($"FSS_TaskForGroup", o => o.TaskGroupId, EumCacheStoreType.Redis);
        public static Task TaskForGroupClear(int taskGroupId) => RedisContext.Instance.CacheManager.RemoveAsync(TaskForGroupKey, taskGroupId);
        public static Task TaskForGroupClear()                => RedisContext.Instance.CacheManager.RemoveAsync(TaskForGroupKey);


        /// <summary> 客户端缓存 </summary>
        public static CacheKey<ClientVO, long> ClientKey => new($"FSS_ClientList", o => o.Id, EumCacheStoreType.Redis);
        public static Task ClientClear(long clientId) => RedisContext.Instance.CacheManager.RemoveAsync(ClientKey, clientId);
    }
}