using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;

namespace FOPS.Com.BuilderServer.Build
{
    public class CheckStep:IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }
        
        public Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"前置检查。");
            
            // 自动创建目录
            BuildLogService.Write(build.Id, $"自动创建目录。");
            if (!System.IO.Directory.Exists(env.FopsDirRoot)) System.IO.Directory.CreateDirectory(env.FopsDirRoot);
            if (!System.IO.Directory.Exists(env.NpmModulesDirRoot)) System.IO.Directory.CreateDirectory(env.NpmModulesDirRoot);
            if (!System.IO.Directory.Exists(env.ReleaseDirRoot)) System.IO.Directory.CreateDirectory(env.ReleaseDirRoot);
            if (!System.IO.Directory.Exists(env.KubePath)) System.IO.Directory.CreateDirectory(env.KubePath);
            if (!System.IO.Directory.Exists(env.ShellScriptPath)) System.IO.Directory.CreateDirectory(env.ShellScriptPath);
            if (!System.IO.Directory.Exists(env.GitDirRoot)) System.IO.Directory.CreateDirectory(env.GitDirRoot);

            // 先删除之前编译的目标文件
            BuildLogService.Write(build.Id, $"先删除之前编译的目标文件。");
            if (System.IO.Directory.Exists(env.ProjectReleaseDirRoot)) System.IO.Directory.Delete(env.ProjectReleaseDirRoot, true);
            System.IO.Directory.CreateDirectory(env.ProjectReleaseDirRoot);
            
            return Task.FromResult(new RunShellResult(false, "前置检查通过。"));
        }
    }
}