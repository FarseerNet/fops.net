using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Git
{
    /// <summary>
    /// Git克隆
    /// </summary>
    public class GitCloneStep : IBuildStep
    {
        public IGitOpr     GitOpr     { get; set; }
        public IGitService GitService { get; set; }

        /// <summary>
        /// Clone代码
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            var url = git.Hub;
            // 需要密码
            if (!string.IsNullOrWhiteSpace(git.UserPwd))
            {
                url = url.Replace("//", $"//{git.UserName.Replace("@", "%40")}:{git.UserPwd}@");
            }

            // 获取Git存放的路径
            var gitPath = GitOpr.GetGitPath(env, git);

            // 让git记住密码
            await ShellTools.Run("git", "config --global credential.helper store", actReceiveOutput, env, null, cancellationToken);

            var result = await ShellTools.Run("git", $"clone -b {git.Branch} {url} {gitPath}", actReceiveOutput, env, null, cancellationToken);
            if (result.IsError)
            {
                return new RunShellResult(true, "Git克隆失败");
            }

            // 更新git拉取时间
            await GitService.UpdateAsync(git.Id, DateTime.Now);
            return new RunShellResult(false, "Git克隆成功");
        }
    }
}