using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Dotnet
{
    public class DotnetBuildStep : IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }
        public IIocManager      IocManager      { get; set; }

        public async Task<RunShellResult> Build(BuildEnvironment env, BuildDTO build, ProjectDTO project, GitDTO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始编译。");

            if (!System.IO.Directory.Exists(env.ProjectSourceDirRoot))
            {
                return new RunShellResult(true, $"路径：{env.ProjectSourceDirRoot}不存在，无法编译");
            }

            // 编译
            var result = await Publish(env, actReceiveOutput, cancellationToken);
            return result.IsError switch
            {
                false => new RunShellResult(false, $"编译完成。"),
                true  => new RunShellResult(true, $"编译出错了。")
            };
        }

        /// <summary>
        /// 编译.net core
        /// </summary>
        private async Task<RunShellResult> Publish(BuildEnvironment env, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            await ShellTools.Run("dotnet", $"restore", actReceiveOutput, env, env.ProjectSourceDirRoot, cancellationToken);
            return await ShellTools.Run("dotnet", $"publish -c Release -o {env.ProjectReleaseDirRoot}", actReceiveOutput, env, env.ProjectSourceDirRoot, cancellationToken);
        }

        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(string savePath, string source, Action<string> actReceiveOutput)
        {
            await ShellTools.Run("dotnet", $"restore", actReceiveOutput, null, source);
            return await ShellTools.Run("dotnet", $"publish -c Release -o {savePath}", actReceiveOutput, null, source);
        }
    }
}