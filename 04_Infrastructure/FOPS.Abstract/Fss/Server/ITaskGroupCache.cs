using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskGroupCache: ISingletonDependency
    {
        /// <summary>
        /// 保存任务组信息
        /// </summary>
        Task SaveAsync(int taskGroupId, TaskGroupVO taskGroup);
    }
}