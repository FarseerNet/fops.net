using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Entity;
using FOPS.Abstract.MetaInfo.Entity;

namespace FOPS.Com.BuilderServer.Docker
{
    public class DockerOpr : IDockerOpr
    {
        /// <summary>
        /// 取得dockerHub
        /// </summary>
        public string GetDockerHub(DockerHubVO docker)
        {
            var dockerHub = "localhost";
            if (docker != null)
            {
                dockerHub = docker.Hub;
                if (dockerHub.EndsWith("/")) dockerHub.Substring(0, dockerHub.Length - 1);
            }

            return dockerHub;
        }

        /// <summary>
        /// 生成镜像名称
        /// </summary>
        public string GetDockerImage(DockerHubVO docker, ProjectVO project, int buildNumber) => $"{GetDockerHub(docker)}:{project.Name}-{buildNumber}";
    }
}