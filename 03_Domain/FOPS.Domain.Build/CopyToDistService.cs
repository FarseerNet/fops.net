using FOPS.Domain.Build.DeployK8S.Entity;
using FS.Utils.Common;

namespace FOPS.Domain.Build
{
    public class CopyToDistService : ISingletonDependency
    {
        public Task<bool> Copy(BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken)
        {
            progress.Report("---------------------------------------------------------");

            // 复制目录
            progress.Report($"源文件{env.ProjectSourceDirRoot}复制到{env.ProjectReleaseDirRoot}");
            Files.CopyFolder(env.ProjectSourceDirRoot, env.ProjectReleaseDirRoot);

            return Task.FromResult(true);
        }
    }
}