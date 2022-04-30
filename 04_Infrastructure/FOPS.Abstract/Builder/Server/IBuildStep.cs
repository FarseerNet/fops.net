using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.MetaInfo.Entity;
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
        Task<RunShellResult> Build(BuildEnvironment dicEnv, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput, CancellationToken cancellationToken);
    }
}