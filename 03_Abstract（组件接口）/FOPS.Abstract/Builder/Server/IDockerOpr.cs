using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Docker.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IDockerOpr: ITransientDependency
    {
        /// <summary>
        /// Docker打包
        /// </summary>
        Task<RunShellResult> Build(BuildEnvironment dicEnv, BuildVO build, ProjectVO project, Action<string> actReceiveOutput);

        /// <summary>
        /// 上传镜像
        /// </summary>
        Task<RunShellResult> Upload(BuildEnvironment dicEnv, BuildVO build, ProjectVO project, Action<string> actReceiveOutput);

        /// <summary>
        /// 取得dockerHub
        /// </summary>
        string GetDockerHub(DockerHubVO docker);

        /// <summary>
        /// 生成镜像名称
        /// </summary>
        string GetDockerImage(DockerHubVO docker, ProjectVO project, int buildNumber);
    }
}