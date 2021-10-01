using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskUpdate: ISingletonDependency
    {
        /// <summary>
        /// 任务组修改时，需要同步JobName
        /// </summary>
        Task UpdateJobName(int taskId, string jobName);

        /// <summary>
        /// 保存Task（taskGroup必须是最新的）
        /// </summary>
        Task SaveFinishAsync(TaskVO task, TaskGroupVO taskGroup);

        /// <summary>
        /// 移除缓存
        /// </summary>
        Task ClearCacheAsync();
    }
}