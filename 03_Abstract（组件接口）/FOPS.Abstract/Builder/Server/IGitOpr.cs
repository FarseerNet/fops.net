using System;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IGitOpr: ITransientDependency
    {
        /// <summary>
        /// 拉取最新代码
        /// </summary>
        Task<RunShellResult> PullAsync(int gitId, Action<string> actReceiveOutput);

        /// <summary>
        /// 消除仓库
        /// </summary>
        void Clear(int gitId);

        /// <summary>
        /// 获取Git存放的路径
        /// </summary>
        string GetGitPath(GitVO info);
    }
}