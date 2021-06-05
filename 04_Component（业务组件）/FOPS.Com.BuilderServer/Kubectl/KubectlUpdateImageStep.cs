using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Kubectl
{
    /// <summary>
    /// K8S更新镜像版本
    /// </summary>
    public class KubectlUpdateImageStep : IBuildStep
    {
        public IClusterService   ClusterService   { get; set; }
        public IKubectlOpr       KubectlOpr       { get; set; }
        public IBuildLogService  BuildLogService  { get; set; }

        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始更新K8S POD的镜像版本。");
            var cluster    = await ClusterService.ToInfoAsync(build.ClusterId);
            var configFile = KubectlOpr.GetConfigFile(cluster.Name);
            KubectlOpr.CreateConfigFile(cluster, configFile);

            // 取得dockerHub
            var result = await ShellTools.Run("kubectl", $"set image deployment/{project.Name} {project.Name}={env.DockerImage} --kubeconfig={configFile}", actReceiveOutput, env);
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
    }
}