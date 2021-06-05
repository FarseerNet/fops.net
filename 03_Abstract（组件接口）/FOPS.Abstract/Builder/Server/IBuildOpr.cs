using System.Threading.Tasks;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IBuildOpr: ITransientDependency
    {
        /// <summary>
        /// 构建
        /// </summary>
        Task Build();
    }
}