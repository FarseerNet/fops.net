using FOPS.Application.Build.DockerHub.Entity;
using FOPS.Application.Build.Project.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IDockerOpr: ISingletonDependency
    {
        /// <summary>
        /// 取得dockerHub
        /// </summary>
        string GetDockerHub(DockerHubDTO docker);

        /// <summary>
        /// 生成镜像名称
        /// </summary>
        string GetDockerImage(DockerHubDTO docker, ProjectDTO project, int buildNumber);
    }
}