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
        public IProjectService  ProjectService  { get; set; }
        public IGitService      GitService      { get; set; }
        public IIocManager      IocManager      { get; set; }

        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, $"构建任务id={build.Id}：开始编译。");

            var savePath                             = SavePath + project.Name;
            var source = project.Path.StartsWith("/") ? project.Path.Substring(1) : project.Path;
            source = GitOpr.GetGitPath(git) + source;
            return await ShellTools.Run("dotnet", $"publish -c Release -o {savePath} {source}", actReceiveOutput);
        }
    }
}