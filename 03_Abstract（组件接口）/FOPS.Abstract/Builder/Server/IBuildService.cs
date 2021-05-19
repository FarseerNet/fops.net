using System.Threading.Tasks;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IBuildService: ITransientDependency
    {
        /// <summary>
        /// 创建构建任务
        /// </summary>
        Task<int> Add(int projectId);

        /// <summary>
        /// 构建
        /// </summary>
        Task Build();

        /// <summary>
        /// 主动取消任务
        /// </summary>
        Task Cancel(int id);
    }
}