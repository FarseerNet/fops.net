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
        /// 获取项目的根目录
        /// </summary>
        string GetSourceDirRoot(BuildEnvironment buildEnvironment, ProjectVO project, GitVO git);

        /// <summary>
        /// 获取编译保存的目录地址
        /// </summary>
        string GetReleasePath(BuildEnvironment env, ProjectVO project);
    }
}