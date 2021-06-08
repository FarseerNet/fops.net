using System;
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
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            // 如果Git存放的目录不存在，则创建
            if (!System.IO.Directory.Exists(env.GitDirRoot)) System.IO.Directory.CreateDirectory(env.GitDirRoot);
            
            var url = git.Hub;
            // 需要密码
            if (!string.IsNullOrWhiteSpace(git.UserPwd))
            {
                url = url.Replace("//", $"//{git.UserName.Replace("@", "%40")}:{git.UserPwd}@");
            }

            // 获取Git存放的路径
            var gitPath = GitOpr.GetGitPath(env,git);

            var result =  await ShellTools.Run("git", $"clone -b {git.Branch} {url} {gitPath}", actReceiveOutput, env);
            if (!result.IsError)
            {
                // 更新git拉取时间
                await GitService.UpdateAsync(git.Id, DateTime.Now);
            }
            return result;
        }
    }
}