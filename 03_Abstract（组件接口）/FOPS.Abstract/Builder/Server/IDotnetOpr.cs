using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IDotnetOpr: ITransientDependency
    {
        /// <summary>
        /// 编译.net core
        /// </summary>
        Task<RunShellResult> Publish(BuildEnvironment dicEnv, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput);

        /// <summary>
        /// 获取项目的根目录
        /// </summary>
        string GetSourceDirRoot(ProjectVO project, GitVO git);

        /// <summary>
        /// 获取编译保存的目录地址
        /// </summary>
        string GetReleasePath(ProjectVO project);
    }
}