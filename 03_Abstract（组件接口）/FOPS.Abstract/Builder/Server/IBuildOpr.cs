using System.Threading.Tasks;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IBuildOpr : ISingletonDependency
    {
        /// <summary>
        /// 构建
        /// </summary>
        Task Build();
    }
}