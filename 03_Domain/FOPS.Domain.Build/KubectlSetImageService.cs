using FOPS.Domain.Build.Build;
using FOPS.Domain.Build.Cluster.Repository;
using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FOPS.Domain.Build.DockerHub.Repository;
using FOPS.Domain.Build.Project;

namespace FOPS.Domain.Build
{
    /// <summary>
    /// K8S更新镜像版本
    /// </summary>
    public class KubectlSetImageService : ISingletonDependency
    {
        public IKubectlDevice       KubectlDevice       { get; set; }
        public IClusterRepository   ClusterRepository   { get; set; }
        public IDockerDevice        DockerDevice        { get; set; }
        public IDockerHubRepository DockerHubRepository { get; set; }

        public async Task<bool> SetImages(BuildEnvironment env, BuildDO build, ProjectDO project, IProgress<string> progress, CancellationToken cancellationToken)
        {
            progress.Report("---------------------------------------------------------");
            progress.Report($"开始更新K8S POD的镜像版本。");
            var cluster = await ClusterRepository.ToInfoAsync(build.ClusterId);
            KubectlDevice.CreateConfigFile(cluster.Name, cluster.Config);

            // 更新镜像
            var result = await KubectlDevice.SetImages(cluster.Name, project.Name, env.DockerImage, project.K8sControllersType, progress, cancellationToken);
            if (result)
            {
                progress.Report("更新镜像版本完成。");
            }
            return result;
        }

        /// <summary>
        /// 同步镜像
        /// </summary>
        public async Task<bool> SyncImages(int clusterId, ProjectDO project, IProgress<string> progress)
        {
            var cluster = await ClusterRepository.ToInfoAsync(clusterId);
            var docker  = await DockerHubRepository.ToInfoAsync(project.DockerHub);

            // 组装镜像版本
            var dockerImage = DockerDevice.GetDockerImage(docker.Hub, project.Name, project.DockerVer.ConvertType(0));
            
            KubectlDevice.CreateConfigFile(cluster.Name, cluster.Config);
            
            // 更新镜像
            var result = await KubectlDevice.SetImages(cluster.Name, project.Name, dockerImage, project.K8sControllersType, progress, default);
            if (result)
            {
                progress.Report("更新镜像版本完成。");
            }
            return result;
        }
    }
}