using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.Utils.Common;

namespace FOPS.Com.BuilderServer.UnBuild
{
    public class CopyToDistStep : IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }

        public Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");

            // 先删除之前编译的目标文件
            if (System.IO.Directory.Exists(env.ProjectReleaseDirRoot))
            {
                BuildLogService.Write(build.Id, $"清除目录：{env.ProjectReleaseDirRoot}。");
                System.IO.Directory.Delete(env.ProjectReleaseDirRoot, true);
            }

            System.IO.Directory.CreateDirectory(env.ProjectReleaseDirRoot);

            // 复制目录
            BuildLogService.Write(build.Id, $"源文件{env.ProjectSourceDirRoot}复制到{env.ProjectReleaseDirRoot}");
            Files.CopyFolder(env.ProjectSourceDirRoot, env.ProjectReleaseDirRoot);

            return Task.FromResult(new RunShellResult(false, "复制完成。"));
        }
    }
}