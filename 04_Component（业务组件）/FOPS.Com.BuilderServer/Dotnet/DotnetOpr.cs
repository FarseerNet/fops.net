using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.BuilderServer.BuildLog;
using FOPS.Infrastructure.Common;
using FS.DI;

namespace FOPS.Com.BuilderServer.Dotnet
{
    public class DotnetOpr : IDotnetOpr
    {
        const string SavePath = "/var/lib/fops/dist/";

        public IGitOpr          GitOpr          { get; set; }
        public IBuildLogService BuildLogService { get; set; }
        public IIocManager      IocManager      { get; set; }

        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始编译。");

            var savePath = SavePath + project.Name;
            var source   = project.Path.StartsWith("/") ? project.Path.Substring(1) : project.Path;
            source = GitOpr.GetGitPath(git) + source;

            // 先删除之前编译的目标文件
            if (System.IO.Directory.Exists(savePath)) System.IO.Directory.Delete(savePath, true);
            System.IO.Directory.CreateDirectory(savePath);

            var result = await Publish(savePath, source, actReceiveOutput);

            switch (result.IsError)
            {
                case false:
                    BuildLogService.Write(build.Id, $"编译完成。");
                    break;
                case true:
                    BuildLogService.Write(build.Id, $"编译出错了。");
                    break;
            }

            return result;
        }

        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(string savePath, string source, Action<string> actReceiveOutput)
        {
            await ShellTools.Run("dotnet",        $"restore",                          actReceiveOutput, source);
            return await ShellTools.Run("dotnet", $"publish -c Release -o {savePath}", actReceiveOutput, source);
        }
    }
}