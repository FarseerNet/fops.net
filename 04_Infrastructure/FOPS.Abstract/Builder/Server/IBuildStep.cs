using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Application.Build.Build.Entity;
using FOPS.Application.Build.Git.Entity;
using FOPS.Application.Build.Project.Entity;
using FS.Core.Entity;

namespace FOPS.Abstract.Builder.Server
{
    /// <summary>
    /// 构建
    /// </summary>
    public interface IBuildStep
    {
        /// <summary>
        /// 构建（具体实现，根据实现类确认）
        /// </summary>
        Task<RunShellResult> Build(BuildEnvironment dicEnv, BuildDTO build, ProjectDTO project, GitDTO git, Action<string> actReceiveOutput, CancellationToken cancellationToken);
    }
}