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

        /// <summary>
        /// 获取存储k8s Config的路径
        /// </summary>
        public string GetConfigFile(BuildEnvironment env, string clusterName) =>$"{env.KubePath}{clusterName}";
        
        /// <summary>
        /// 更新k8s版本
        /// </summary>
        public async Task<RunShellResult> SetImages(int clusterId, int buildNumber, ProjectVO project, Action<string> actReceiveOutput)
        {
            var cluster    = await ClusterService.ToInfoAsync(clusterId);
            var env        = new BuildEnvironment();
            var configFile = GetConfigFile(env,cluster.Name);
            CreateConfigFile(env,cluster);
            // Docker仓库，如果配置了，说明需要上传，则镜像名要设置前缀
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);

            // 取得dockerHub
            var dockerName = DockerOpr.GetDockerImage(docker, project, buildNumber);
            return await ShellTools.Run("kubectl", $"set image deployment/{project.Name} {project.Name}={dockerName} --kubeconfig={configFile}", actReceiveOutput, null);
        }

        /// <summary>
        /// 创建K8S集群的配置
        /// </summary>
        public void CreateConfigFile(BuildEnvironment env, ClusterVO cluster)
        {
            var configFile = GetConfigFile(env, cluster.Name);
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