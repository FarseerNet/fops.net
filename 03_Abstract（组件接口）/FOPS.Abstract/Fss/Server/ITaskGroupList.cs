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
        /// 删除整个缓存
        /// </summary>
        Task ClearAsync();

        /// <summary>
        /// 获取全部任务列表
        /// </summary>
        Task<List<TaskGroupVO>> ToListAndSaveAsync();
    }
}