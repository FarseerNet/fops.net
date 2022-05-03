using FOPS.Domain.Build.Build;
using FOPS.Domain.Build.Cluster.Repository;
using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.DeployK8S.Entity;
using FOPS.Domain.Build.Project;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Domain.Build
{
    /// <summary>
    /// K8S更新镜像版本
    /// </summary>
    public class KubectlSetImageService : ISingletonDependency
    {
        public IKubectlDevice     KubectlDevice     { get; set; }
        public IClusterRepository ClusterRepository { get; set; }

        public async Task<bool> SetImage(BuildEnvironment env, BuildDO build, ProjectDO project, IProgress<string> progress, CancellationToken cancellationToken)
        {
            progress.Report("---------------------------------------------------------");
            progress.Report($"开始更新K8S POD的镜像版本。");
            var cluster = await ClusterRepository.ToInfoAsync(build.ClusterId);
            KubectlDevice.CreateConfigFile(cluster.Name, cluster.Config);

            // 更新镜像
            var result = await KubectlDevice.SetImages(cluster.Name, project.K8sControllersType, env, progress, cancellationToken);
            if (result)
            {
                progress.Report("更新镜像版本完成。");
            }
            return result;
        }
    }
}