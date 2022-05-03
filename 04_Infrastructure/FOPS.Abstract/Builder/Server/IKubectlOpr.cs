using System;
using System.Threading.Tasks;
using FOPS.Application.Build.Cluster.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Domain.Build.Deploy.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IKubectlOpr: ISingletonDependency
    {
        /// <summary>
        /// 更新k8s版本
        /// </summary>
        Task<RunShellResult> SetImages(int clusterId, int buildNumber, ProjectDTO project, Action<string> actReceiveOutput);

        /// <summary>
        /// 创建K8S集群的配置
        /// </summary>
        void CreateConfigFile(BuildEnvironment env, ClusterDTO cluster);

        /// <summary>
        /// 获取存储k8s Config的路径
        /// </summary>
        string GetConfigFile(BuildEnvironment env, string clusterName);
    }
}