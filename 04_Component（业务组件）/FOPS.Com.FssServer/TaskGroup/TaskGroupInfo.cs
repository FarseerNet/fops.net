using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FS.Cache.Redis;
using FS.DI;
using FS.Extends;

namespace FOPS.Com.FssServer.TaskGroup
{
    public class TaskGroupInfo : ITaskGroupInfo
    {
        public ITaskGroupAgent TaskGroupAgent { get; set; }
        public IIocManager     IocManager     { get; set; }
        
        private IRedisCacheManager RedisCacheManager => IocManager.Resolve<IRedisCacheManager>("fss_redis");

        /// <summary>
        /// 获取任务信息
        /// </summary>
        public Task<TaskGroupVO> ToInfoAsync(int id)
        {
            return RedisCacheManager.CacheManager.ToEntityAsync<TaskGroupVO>(TaskGroupCache.Key,
                id.ToString(),
                _ => TaskGroupAgent.ToEntityAsync(id).MapAsync<TaskGroupVO, TaskGroupPO>(),
                o => o.Id);
        }

        /// <summary>
        /// 从数据库中取出并保存
        /// </summary>
        public async Task<TaskGroupVO> ToInfoByDbAsync(int id)
        {
            var entity = await TaskGroupAgent.ToEntityAsync(id).MapAsync<TaskGroupVO, TaskGroupPO>();
            await RedisCacheManager.CacheManager.SaveAsync(TaskGroupCache.Key,
                entity,
                id.ToString());
            return entity;
        }
    }
}