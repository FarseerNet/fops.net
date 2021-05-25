using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskList: ITransientDependency
    {
        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        Task<List<TaskVO>> ToTopListAsync(int top);

        /// <summary>
        /// 获取指定任务组的任务列表
        /// </summary>
        Task<List<TaskVO>> ToListAsync(int groupId, int pageSize, int pageIndex, out int totalCount);

        /// <summary>
        /// 获取失败的任务列表
        /// </summary>
        Task<List<TaskVO>> ToFailListAsync(int pageSize, int pageIndex, out int totalCount);

        /// <summary>
        /// 获取未执行的任务列表
        /// </summary>
        Task<List<TaskVO>> ToUnRunListAsync(int pageSize, int pageIndex, out int totalCount);

        /// <summary>
        /// 获取指定任务组执行成功的任务列表
        /// </summary>
        Task<List<TaskVO>> ToSuccessListAsync(int groupId, int top);

        /// <summary>
        /// 清除成功的任务记录（1天前）
        /// </summary>
        Task ClearSuccessAsync(int groupId, int taskId);

        /// <summary>
        /// 获取失败的任务数量
        /// </summary>
        Task<int> TodayFailCountAsync();
    }
}