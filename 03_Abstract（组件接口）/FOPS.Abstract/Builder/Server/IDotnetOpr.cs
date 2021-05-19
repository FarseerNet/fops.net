using System;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IDotnetOpr: ITransientDependency
    {
        /// <summary>
        /// 编译.net core
        /// </summary>
        Task<RunShellResult> Publish(string project, string source, Action<string> actReceiveOutput);
    }
}