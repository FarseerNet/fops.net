using System.Threading.Tasks;
using FS.DI;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface ITaskGroupDelete: ISingletonDependency
    {
        /// <summary>
        /// 删除任务组
        /// </summary>
        Task DeleteAsync(int taskGroupId);
    }
}