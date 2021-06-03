using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Kubectl
{
    public class KubectlOpr : IKubectlOpr
    {
        public IClusterService   ClusterService   { get; set; }
        public IDockerHubService DockerHubService { get; set; }
        public IDockerOpr        DockerOpr        { get; set; }
        public IBuildLogService  BuildLogService  { get; set; }
        const  string            SavePath = "/var/lib/fops/kube/";

        /// <summary>
        /// 更新k8s版本k
        /// </summary>
        public async Task<RunShellResult> SetImages(BuildEnvironment env, BuildVO build, ProjectVO project, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始更新K8S POD的镜像版本。");
            var cluster    = await ClusterService.ToInfoAsync(build.ClusterId);
            var configFile = $"{SavePath}{cluster.Name}";
            CreateConfigFile(cluster, configFile);

            // Docker仓库，如果配置了，说明需要上传，则镜像名要设置前缀
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);

            // 取得dockerHub
            var dockerHub  = DockerOpr.GetDockerHub(docker);
            var dockerName = $"{dockerHub}:{project.Name}-{build.BuildNumber}";
            var result     = await ShellTools.Run("kubectl", $"set image deployment/{project.Name} {project.Name}={dockerName} --kubeconfig={configFile}", actReceiveOutput, env);
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
        public async Task<RunShellResult> SetImages(int clusterId, string dockerVer, ProjectVO project, Action<string> actReceiveOutput)
        {
            var cluster    = await ClusterService.ToInfoAsync(clusterId);
            var configFile = $"{SavePath}{cluster.Name}";
            CreateConfigFile(cluster, configFile);
            // Docker仓库，如果配置了，说明需要上传，则镜像名要设置前缀
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);

            // 取得dockerHub
            var dockerHub  = DockerOpr.GetDockerHub(docker);
            var dockerName = $"{dockerHub}:{project.Name}-{dockerVer}";
            return await ShellTools.Run("kubectl", $"set image deployment/{project.Name} {project.Name}={dockerName} --kubeconfig={configFile}", actReceiveOutput, null);
        }

        /// <summary>
        /// 创建K8S集群的配置
        /// </summary>
        private void CreateConfigFile(ClusterVO cluster, string configFile)
        {
            if (!System.IO.Directory.Exists(SavePath)) System.IO.Directory.CreateDirectory(SavePath);

            // 文件不存在，则创建
            if (!System.IO.File.Exists(configFile))
            {
                System.IO.File.WriteAllText(configFile, cluster.Config);
            }
            else
            {
                // 比对配置是否不一样，不一样则覆盖新的
                var config = System.IO.File.ReadAllText(configFile);
                if (cluster.Config != config)
                {
                    System.IO.File.WriteAllText(configFile, cluster.Config);
                }
            }
        }
    }
}