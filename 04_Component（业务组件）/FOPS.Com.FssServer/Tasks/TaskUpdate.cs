using System.Threading.Tasks;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Tasks.Dal;
using FS.Cache;
using FS.Cache.Redis;
using FS.DI;

namespace FOPS.Com.FssServer.Tasks
{
    public class TaskUpdate : ITaskUpdate
    {
        public  ITaskInfo          TaskInfo          { get; set; }
        public  IIocManager        IocManager        { get; set; }
        private IRedisCacheManager RedisCacheManager => IocManager.Resolve<IRedisCacheManager>("fss_redis");
        
        /// <summary>
        /// 任务组修改时，需要同步JobName
        /// </summary>
        public async Task UpdateJobName(int id, string jobName)
        {
            await FssContext.Data.Task.Where(o => o.Id == id).UpdateAsync(new TaskPO() {JobName = jobName});
            var task = await TaskInfo.ToInfoByDbAsync(id);
            await RedisCacheManager.CacheManager.SaveAsync(TaskCache.Key, task, task.TaskGroupId, new CacheOption());
        }
    }
}