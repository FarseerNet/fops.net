using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Shell
{
    /// <summary>
    /// 构建shell脚本
    /// </summary>
    public class ShellStep: IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }
        
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始执行Shell脚本。");
            BuildLogService.Write(build.Id, $"请注意：脚本执行完后，请自行将打包的文件复制到：{env.ProjectReleaseDirRoot}。");
            
            // 每次执行时，需要生成shell脚本
            var path = env.ShellScriptPath + $"fops_{build.Id}.sh";
            System.IO.File.AppendAllText(path, project.ShellScript);
            
            // 执行脚本
            var result = await ShellTools.Run("/bin/sh", $"-xe {path}", actReceiveOutput, env,env.ProjectReleaseDirRoot);
            
            return result.IsError switch
            {
                false => new RunShellResult(false, "执行脚本完成。"),
                true  => new RunShellResult(true,  "执行脚本出错了。")
            };
        }
    }
}