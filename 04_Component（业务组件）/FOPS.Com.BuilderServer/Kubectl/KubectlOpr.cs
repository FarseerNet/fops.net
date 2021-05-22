using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Infrastructure.Common;
using FS.Core.Entity;

namespace FOPS.Com.BuilderServer.Kubectl
{
    public class KubectlOpr : IKubectlOpr
    {
        public IClusterService   ClusterService   { get; set; }
        public IDockerHubService DockerHubService { get; set; }
        public IDockerOpr        DockerOpr        { get; set; }
        public IBuildLogService  BuildLogService  { get; set; }
        
        /// <summary>
        /// 更新k8s版本
        /// </summary>
        public async Task<RunShellResult> SetImages(BuildVO build, ProjectVO project, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始更新K8S POD的镜像版本。");
            var cluster = await ClusterService.ToInfoAsync(build.ClusterId);

            // Docker仓库，如果配置了，说明需要上传，则镜像名要设置前缀
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);
            
            // 取得dockerHub
            var dockerHub  = DockerOpr.GetDockerHub(docker);
            var dockerName = $"{dockerHub}:{project.Name}-{build.BuildNumber}";
            var result= await ShellTools.Run("kubectl", $"set image deployment/{project.Name} {project.Name}={dockerName} --kubeconfig={cluster.ConfigName}", actReceiveOutput);
            switch (result.IsError)
            {
                case false:
                    BuildLogService.Write(build.Id, $"更新镜像版本完成。");
                    break;
                case true:
                    BuildLogService.Write(build.Id, $"更新镜像版本出错了。");
                    break;
            }

            return result;
        }
        
        /// <summary>
        /// 更新k8s版本
        /// </summary>
        public async Task<RunShellResult> SetImages(int clusterId,string dockerVer, ProjectVO project, Action<string> actReceiveOutput)
        {
            var cluster = await ClusterService.ToInfoAsync(clusterId);

            // Docker仓库，如果配置了，说明需要上传，则镜像名要设置前缀
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);
            
            // 取得dockerHub
            var dockerHub  = DockerOpr.GetDockerHub(docker);
            var dockerName = $"{dockerHub}:{project.Name}-{dockerVer}";
            return await ShellTools.Run("kubectl", $"set image deployment/{project.Name} {project.Name}={dockerName} --kubeconfig={cluster.ConfigName}", actReceiveOutput);
        }
    }
}