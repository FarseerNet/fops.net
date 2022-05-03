using FOPS.Domain.Build.DeployK8S.Entity;
using FOPS.Domain.Build.Enum;

namespace FOPS.Domain.Build.Deploy.Device;

/// <summary>
/// Kubectl命令驱动接口
/// </summary>
public interface IKubectlDevice: ISingletonDependency
{
    /// <summary>
    /// 获取存储k8s Config的路径
    /// </summary>
    string GetConfigFile(string clusterName);
    /// <summary>
    /// 创建用于K8S远程管理的配置文件
    /// </summary>
    void CreateConfigFile(string clusterName, string clusterConfig);
    /// <summary>
    /// 更新k8s的镜像版本
    /// </summary>
    Task<bool> SetImages(string clusterName, EumK8sControllers k8sControllersType, BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken);
    /// <summary>
    /// 生成yaml文件，并执行kubectl apply命令
    /// </summary>
    Task<bool> SetYaml(string clusterName, string projectName, string yamlContent, IProgress<string> progress, CancellationToken cancellationToken);
}