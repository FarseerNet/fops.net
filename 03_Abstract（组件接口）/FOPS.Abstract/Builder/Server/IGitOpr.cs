using System;
using System.Collections.Generic;
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
        string GetGitPath(BuildEnvironment env, GitVO info);
        
        /// <summary>
        /// 根据判断是否存在Git目录，来决定返回Clone or pull
        /// </summary>
        IBuildStep GetGitStep(string gitDirRoot);

        /// <summary>
        /// 根据判断是否存在Git目录，来决定返回Clone or pull
        /// </summary>
        Task<RunShellResult> CloneOrPull(GitVO git);
    }
}