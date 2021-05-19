using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FS.DI;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IGitOpr: ITransientDependency
    {
        /// <summary>
        /// 拉取最新代码
        /// </summary>
        Task<RunShellResult> PullAsync(int gitId);
    }
}