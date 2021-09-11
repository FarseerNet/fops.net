using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskAdd: ITransientDependency
    {
        /// <summary>
        /// 创建Task，并更新到缓存
        /// </summary>
        Task<TaskVO> GetOrCreateAsync(int taskGroupId);

        /// <summary>
        /// 创建Task，并更新到缓存
        /// </summary>
        Task<TaskVO> GetOrCreateAsync(TaskGroupVO taskGroup);
    }
}