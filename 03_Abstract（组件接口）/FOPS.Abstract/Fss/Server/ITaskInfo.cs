using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskInfo: ISingletonDependency
    {
        /// <summary>
        /// 获取任务信息
        /// </summary>
        Task<TaskVO> ToInfoByDbAsync(int id);

        /// <summary>
        /// 今日执行失败数量
        /// </summary>
        Task<int> TodayFailCountAsync();

        /// <summary>
        /// 获取所有任务组
        /// </summary>
        Task<List<TaskVO>> ToGroupListAsync();
        /// <summary>
        /// 获取当前任务组的任务
        /// </summary>
        Task<TaskVO> ToInfoByGroupIdAsync(int taskGroupId);
        /// <summary>
        /// 计算任务的平均运行速度
        /// </summary>
        Task<long> StatAvgSpeedAsync(int taskGroupId);
    }
}