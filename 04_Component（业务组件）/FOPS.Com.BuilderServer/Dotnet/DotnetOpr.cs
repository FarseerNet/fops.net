using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.BuilderServer.BuildLog;
using FOPS.Infrastructure.Common;
using FS.Core.Entity;
using FS.DI;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Dotnet
{
    public class DotnetOpr : IDotnetOpr
    {
        const string SavePath = "/var/lib/fops/dist/";

        public IGitOpr          GitOpr          { get; set; }
        public IBuildLogService BuildLogService { get; set; }
        public IIocManager      IocManager      { get; set; }

        /// <summary>
        /// 获取项目源地址
        /// </summary>
        public string GetSourceDirRoot(ProjectVO project, GitVO git) => GitOpr.GetGitPath(git) + (project.Path.StartsWith("/") ? project.Path.Substring(1) : project.Path);

        /// <summary>
        /// 获取编译保存的目录地址
        /// </summary>
        public string GetReleasePath(ProjectVO project) => SavePath + project.Name;

        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(BuildEnvironment env, BuildVO build, ProjectVO project, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始编译。");

            if (!System.IO.Directory.Exists(env.ProjectSourceDirRoot))
            {
                var log = $"路径：{env.ProjectSourceDirRoot}不存在，无法编译";
                BuildLogService.Write(build.Id, log);
                return new RunShellResult
                {
                    IsError = true,
                    Output  = new List<string> {log}
                };
            }

            // 先删除之前编译的目标文件
            if (System.IO.Directory.Exists(env.ProjectReleaseDirRoot)) System.IO.Directory.Delete(env.ProjectReleaseDirRoot, true);
            System.IO.Directory.CreateDirectory(env.ProjectReleaseDirRoot);

            // 编译
            var result = await Publish(env, actReceiveOutput);
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
        public async Task<RunShellResult> Publish(BuildEnvironment env, Action<string> actReceiveOutput)
        {
            await ShellTools.Run("dotnet",        $"restore",                                              actReceiveOutput, env, env.ProjectSourceDirRoot);
            return await ShellTools.Run("dotnet", $"publish -c Release -o {env.ProjectReleaseDirRoot}", actReceiveOutput, env, env.ProjectSourceDirRoot);
        }

        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(string savePath, string source, Action<string> actReceiveOutput)
        {
            await ShellTools.Run("dotnet",        $"restore",                          actReceiveOutput, null, source);
            return await ShellTools.Run("dotnet", $"publish -c Release -o {savePath}", actReceiveOutput, null, source);
        }
    }
}