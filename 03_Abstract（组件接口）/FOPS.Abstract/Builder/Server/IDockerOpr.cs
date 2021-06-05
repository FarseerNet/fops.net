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
        /// 取得dockerHub
        /// </summary>
        string GetDockerHub(DockerHubVO docker);

        /// <summary>
        /// 生成镜像名称
        /// </summary>
        string GetDockerImage(DockerHubVO docker, ProjectVO project, int buildNumber);
    }
}