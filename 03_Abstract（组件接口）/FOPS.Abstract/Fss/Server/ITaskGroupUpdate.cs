using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskGroupUpdate: ITransientDependency
    {
        /// <summary>
        /// 更新TaskGroup
        /// </summary>
        Task UpdateAsync(TaskGroupVO taskGroup);

        /// <summary>
        /// 保存TaskGroup
        /// </summary>
        Task SaveAsync(TaskGroupVO taskGroup);
    }
}