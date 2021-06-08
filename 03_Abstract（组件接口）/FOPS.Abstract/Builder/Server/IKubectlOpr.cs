using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IKubectlOpr: ITransientDependency
    {
        /// <summary>
        /// 更新k8s版本
        /// </summary>
        Task<RunShellResult> SetImages(int clusterId, int buildNumber, ProjectVO project, Action<string> actReceiveOutput);

        /// <summary>
        /// 创建K8S集群的配置
        /// </summary>
        void CreateConfigFile(BuildEnvironment env, ClusterVO cluster, string configFile);

        /// <summary>
        /// 获取存储k8s Config的路径
        /// </summary>
        string GetConfigFile(BuildEnvironment env, string clusterName);
    }
}