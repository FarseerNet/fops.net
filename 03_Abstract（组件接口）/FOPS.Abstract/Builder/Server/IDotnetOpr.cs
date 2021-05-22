using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IDotnetOpr: ITransientDependency
    {
        /// <summary>
        /// 编译.net core
        /// </summary>
        Task<RunShellResult> Publish(BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput);
    }
}