using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FOPS.Domain.Build.Git.Repository;
using FOPS.Domain.Build.Project;
using FS.Utils.Common;

namespace FOPS.Domain.Build
{
    public class CopyToDistService : ISingletonDependency
    {
        public IGitRepository GitRepository { get; set; }
        public IGitDevice     GitDevice     { get; set; }

        public async Task<bool> Copy(ProjectDO project, BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken)
        {
            progress.Report("---------------------------------------------------------");

            // 主项目
            progress.Report($"源文件{env.ProjectGitRoot} 复制到 {BuildEnvironment.DistRoot}{env.ProjectGitRoot.Split('/')[^2]}");
            Files.CopyFolder(env.ProjectGitRoot, BuildEnvironment.DistRoot + env.ProjectGitRoot.Split('/')[^2]);

            // 依赖项目
            var lstGit = await GitRepository.ToListAsync(project.DependentGitIds);
            foreach (var git in lstGit)
            {
                var projectPath = GitDevice.GetGitPath(git.Hub);
                progress.Report($"源文件{projectPath} 复制到 {BuildEnvironment.DistRoot}{projectPath.Split('/')[^2]}");
                Files.CopyFolder(projectPath, BuildEnvironment.DistRoot + projectPath.Split('/')[^2]);
            }

            return true;
        }
    }
}