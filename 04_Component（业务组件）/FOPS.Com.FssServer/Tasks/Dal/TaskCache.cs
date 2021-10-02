using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Infrastructure.Repository;
using FS.DI;

namespace FOPS.Com.FssServer.Tasks.Dal
{
    /// <summary>
    /// 任务缓存
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class TaskCache : ISingletonDependency
    {
        public static string FailKey(int groupId) => $"FSS_Task_Fail:{groupId}";

        /// <summary>
        /// 保存任务信息
        /// </summary>
        public Task SaveAsync(TaskVO task)
        {
            var key = CacheKeys.TaskForGroupKey;
            return RedisContext.Instance.CacheManager.SaveItemAsync(key, task, task.TaskGroupId);
        }
    }
}