using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskGroupList: ITransientDependency
    {
        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        Task<List<TaskGroupVO>> ToListAsync();

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        Task<List<TaskGroupVO>> ToListAndSaveAsync();

        /// <summary>
        /// 获取任务组数量
        /// </summary>
        Task<long> Count();

        /// <summary>
        /// 获取未执行的任务列表
        /// </summary>
        Task<int> ToUnRunCountAsync();
    }
}