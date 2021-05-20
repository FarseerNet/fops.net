using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IDockerOpr: ITransientDependency
    {
        /// <summary>
        /// Docker打包
        /// </summary>
        Task<RunShellResult> Build(BuildVO build, ProjectVO project, Action<string> actReceiveOutput);

        /// <summary>
        /// 上传镜像
        /// </summary>
        Task<RunShellResult> Upload(BuildVO build, ProjectVO project, Action<string> actReceiveOutput);
    }
}