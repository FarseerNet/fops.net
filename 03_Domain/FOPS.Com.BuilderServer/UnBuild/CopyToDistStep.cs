using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Server;
using FOPS.Application.Build.Build.Entity;
using FOPS.Application.Build.Git.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Domain.Build.DeployK8S.Entity;
using FS.Core.Entity;
using FS.Utils.Common;

namespace FOPS.Com.BuilderServer.UnBuild
{
    public class CopyToDistStep : IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }

        public Task<RunShellResult> Build(BuildEnvironment env, BuildDTO build, ProjectDTO project, GitDTO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");

            // 复制目录
            BuildLogService.Write(build.Id, $"源文件{env.ProjectSourceDirRoot}复制到{env.ProjectReleaseDirRoot}");
            Files.CopyFolder(env.ProjectSourceDirRoot, env.ProjectReleaseDirRoot);

            return Task.FromResult(new RunShellResult(false, "复制完成。"));
        }
    }
}