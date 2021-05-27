using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IGitOpr: ITransientDependency
    {
        /// <summary>
        /// 消除仓库
        /// </summary>
        Task<RunShellResult> ClearAsync(int gitId);

        /// <summary>
        /// 获取Git存放的路径
        /// </summary>
        string GetGitPath(GitVO info);

        /// <summary>
        /// 拉取最新代码
        /// </summary>
        Task<RunShellResult> PullAsync(BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput);

        /// <summary>
        /// 拉取最新代码
        /// </summary>
        Task<RunShellResult> PullAsync(GitVO git, Action<string> actReceiveOutput);
    }
}