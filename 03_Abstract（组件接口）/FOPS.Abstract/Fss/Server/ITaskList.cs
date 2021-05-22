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
        /// 获取指定任务组执行成功的任务列表
        /// </summary>
        Task<List<TaskVO>> ToSuccessListAsync(int groupId, int top);

        /// <summary>
        /// 清除成功的任务记录（1天前）
        /// </summary>
        Task ClearSuccessAsync(int groupId, int taskId);
    }
}