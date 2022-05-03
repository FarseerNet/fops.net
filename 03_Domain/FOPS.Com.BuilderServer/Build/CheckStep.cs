using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Server;
using FOPS.Application.Build.Build.Entity;
using FOPS.Application.Build.Git.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Domain.Build.DeployK8S.Entity;
using FS.Core.Entity;

namespace FOPS.Com.BuilderServer.Build
{
    public class CheckStep : IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }

        public Task<RunShellResult> Build(BuildEnvironment env, BuildDTO build, ProjectDTO project, GitDTO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"前置检查。");

            // 自动创建目录
            BuildLogService.Write(build.Id, $"自动创建目录。");
            if (!System.IO.Directory.Exists(BuildEnvironment.FopsDirRoot)) System.IO.Directory.CreateDirectory(BuildEnvironment.FopsDirRoot);
            if (!System.IO.Directory.Exists(BuildEnvironment.NpmModulesDirRoot)) System.IO.Directory.CreateDirectory(BuildEnvironment.NpmModulesDirRoot);
            if (!System.IO.Directory.Exists(BuildEnvironment.ReleaseDirRoot)) System.IO.Directory.CreateDirectory(BuildEnvironment.ReleaseDirRoot);
            if (!System.IO.Directory.Exists(BuildEnvironment.KubePath)) System.IO.Directory.CreateDirectory(BuildEnvironment.KubePath);
            if (!System.IO.Directory.Exists(BuildEnvironment.ShellScriptPath)) System.IO.Directory.CreateDirectory(BuildEnvironment.ShellScriptPath);
            if (!System.IO.Directory.Exists(BuildEnvironment.GitDirRoot)) System.IO.Directory.CreateDirectory(BuildEnvironment.GitDirRoot);

            // 先删除之前编译的目标文件
            BuildLogService.Write(build.Id, $"先删除之前编译的目标文件。");
            if (System.IO.Directory.Exists(env.ProjectReleaseDirRoot)) System.IO.Directory.Delete(env.ProjectReleaseDirRoot, true);
            System.IO.Directory.CreateDirectory(env.ProjectReleaseDirRoot);

            return Task.FromResult(new RunShellResult(false, "前置检查通过。"));
        }
    }
}