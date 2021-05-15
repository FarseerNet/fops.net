using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.Tasks.Dal;
using FS.Cache.Redis;
using FS.DI;
using FS.Extends;

namespace FOPS.Com.FssServer.Tasks
{
    public class TaskInfo : ITaskInfo
    {
        public ITaskAgent  TaskAgent  { get; set; }
        public IIocManager IocManager { get; set; }
        
        private IRedisCacheManager RedisCacheManager => IocManager.Resolve<IRedisCacheManager>("fss_redis");
        public  ITaskAdd           TaskAdd           { get; set; }
        public  ITaskGroupList     TaskGroupList     { get; set; }

        /// <summary>
        /// 获取任务信息
        /// </summary>
        public Task<TaskVO> ToInfoByDbAsync(int id) => TaskAgent.ToEntityAsync(id).MapAsync<TaskVO, TaskPO>();
        
        /// <summary>
        /// 获取当前任务组的任务
        /// </summary>
        public Task<TaskVO> ToGroupAsync(int taskGroupId)
        {
            return RedisCacheManager.CacheManager.ToEntityAsync(TaskCache.Key,
                taskGroupId.ToString(),
                o => TaskAdd.GetOrCreateAsync(taskGroupId),
                o => o.TaskGroupId);
        }

        /// <summary>
        /// 获取所有任务组
        /// </summary>
        public Task<List<TaskVO>> ToGroupListAsync()
        {
            return RedisCacheManager.CacheManager.GetListAsync<TaskVO>(TaskCache.Key,
                async _ =>
                {
                    var taskGroupVos = await TaskGroupList.ToListAsync();
                    var lst          = new List<TaskVO>();
                    foreach (var taskGroupVo in taskGroupVos)
                    {
                        lst.Add(await TaskAdd.GetOrCreateAsync(taskGroupVo.Id));
                    }
                    return lst;
                }, o => o.TaskGroupId);
        }
        
        /// <summary>
        /// 今日执行失败数量
        /// </summary>
        public Task<int> TodayFailCountAsync() => TaskAgent.TodayFailCountAsync();
    }
}