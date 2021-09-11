using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FS.Cache.Redis;
using Newtonsoft.Json;

namespace FOPS.Com.FssServer.TaskGroup.Dal
{
    /// <summary>
    /// 任务组缓存
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class TaskGroupCache : ITaskGroupCache
    {
        public IRedisCacheManager RedisCacheManager { get; set; }

        public const string Key = "FSS_TaskGroup";

        /// <summary>
        /// 保存任务组信息
        /// </summary>
        public Task SaveAsync(int taskGroupId, TaskGroupVO taskGroup)
        {
            return RedisCacheManager.Db.HashSetAsync(Key, taskGroupId, JsonConvert.SerializeObject(taskGroup));
        }
    }
}