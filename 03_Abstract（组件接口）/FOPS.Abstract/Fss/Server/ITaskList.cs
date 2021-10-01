using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskList: ISingletonDependency
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
    }
}