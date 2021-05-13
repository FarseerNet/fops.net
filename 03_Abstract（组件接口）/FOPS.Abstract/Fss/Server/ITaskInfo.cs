using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskInfo: ITransientDependency
    {
        /// <summary>
        /// 获取任务信息
        /// </summary>
        Task<TaskVO> ToInfoByDbAsync(int id);

        /// <summary>
        /// 获取当前任务组的任务
        /// </summary>
        Task<TaskVO> ToGroupAsync(int taskGroupId);

        /// <summary>
        /// 获取所有任务组
        /// </summary>
        Task<List<TaskVO>> ToGroupListAsync();
    }
}