using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Build
{
    public class CheckStep:IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }
        
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"前置检查。");
            
            // 判断目录是否存在（dotnet publish、不编译选项，都实现了创建）
            if (!System.IO.Directory.Exists(env.ProjectReleaseDirRoot)) System.IO.Directory.CreateDirectory(env.ProjectReleaseDirRoot);
            
            if (!System.IO.Directory.Exists(env.ProjectSourceDirRoot))
            {
                return new RunShellResult(true, $"源文件：{env.ProjectSourceDirRoot} 不存在，请检查项目设置。");
            }
            return new RunShellResult(false, "前置检查通过。");
        }
    }
}