using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FS.Core.Entity;
using FS.DI;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Git
{
    public class GitOpr : IGitOpr
    {
        public IGitService GitService { get; set; }
        public IIocManager IocManager { get; set; }

        /// <summary>
        /// 获取Git存放的路径
        /// </summary>
        public string GetGitPath(BuildEnvironment env, GitDTO info)
        {
            var gitName                           = info.Hub.Substring(info.Hub.LastIndexOf('/') + 1);
            if (gitName.EndsWith(".git")) gitName = gitName.Substring(0, gitName.Length - 4);
            return env.GitDirRoot + gitName + "/";
        }

        /// <summary>
        /// 根据判断是否存在Git目录，来决定返回Clone or pull
        /// </summary>
        public async Task<RunShellResult> CloneOrPull(GitDTO git)
        {
            var env        = new BuildEnvironment();
            var gitDirRoot = GetGitPath(env, git);
            var result     = await GetGitStep(gitDirRoot).Build(null, null, null, git, null, default);
            if (!result.IsError)
            {
                // 更新git拉取时间
                await GitService.UpdateAsync(git.Id, DateTime.Now);
            }
            return result;
        }

        /// <summary>
        /// 根据判断是否存在Git目录，来决定返回Clone or pull
        /// </summary>
        public IBuildStep GetGitStep(string gitDirRoot)
        {
            // 如果Git存放的目录不存在，则创建
            var env = new BuildEnvironment();
            if (!System.IO.Directory.Exists(env.GitDirRoot)) System.IO.Directory.CreateDirectory(env.GitDirRoot);

            // 目录不存在，返回clone
            if (!System.IO.Directory.Exists(gitDirRoot))
            {
                return IocManager.Resolve<GitCloneStep>();
            }

            // 返回 pull
            return IocManager.Resolve<GitPullStep>();
        }

        /// <summary>
        /// 消除仓库
        /// </summary>
        public async Task<RunShellResult> ClearAsync(int gitId)
        {
            var info = await GitService.ToInfoAsync(gitId);

            // 获取Git存放的路径
            var env     = new BuildEnvironment();
            var gitPath = GetGitPath(env, info);
            return await ShellTools.Run("rm", $"-rf {gitPath}", null, null);
        }
    }
}