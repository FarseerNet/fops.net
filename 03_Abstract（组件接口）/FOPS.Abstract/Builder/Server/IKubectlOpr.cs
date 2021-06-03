using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IKubectlOpr: ITransientDependency
    {
        /// <summary>
        /// 更新k8s版本
        /// </summary>
        Task<RunShellResult> SetImages(BuildEnvironment buildEnvironment, BuildVO build, ProjectVO project, Action<string> actReceiveOutput);

        /// <summary>
        /// 更新k8s版本
        /// </summary>
        Task<RunShellResult> SetImages(int clusterId, string dockerVer, ProjectVO project, Action<string> actReceiveOutput);
    }
}