using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Domain.Build.Enum;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Git
{
    public class GitPullStep : IBuildStep
    {
        public IBuildService BuildService { get; set; }
        public IGitOpr       GitOpr       { get; set; }
        public IGitService   GitService   { get; set; }

        /// <summary>
        /// 拉取最新代码
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            build = await BuildService.ToInfoAsync(build.Id);
            if (build.Status == EumBuildStatus.Finish) return new RunShellResult(true, "手动取消");

            // 获取Git存放的路径
            var gitPath = GitOpr.GetGitPath(env, git);
            var result  = await ShellTools.Run("git", $"-C {gitPath} pull --rebase", actReceiveOutput, env, null, cancellationToken);
            if (result.IsError)
            {
                return new RunShellResult(true, "Git拉取失败");
            }

            // 更新git拉取时间
            await GitService.UpdateAsync(git.Id, DateTime.Now);
            return new RunShellResult(false, "Git拉取成功");
        }
    }
}