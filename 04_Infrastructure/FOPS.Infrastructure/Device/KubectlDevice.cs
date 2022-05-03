using System.Text;
using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FOPS.Domain.Build.Enum;
using FOPS.Infrastructure.Repository.DockerHub;
using FS.Extends;
using FS.Utils.Component;

namespace FOPS.Infrastructure.Device;

/// <summary>
/// Kubectl工具
/// </summary>
public class KubectlDevice : IKubectlDevice
{
    /// <summary>
    /// 获取存储k8s Config的路径
    /// </summary>
    public string GetConfigFile(string clusterName) => $"{BuildEnvironment.KubePath}{clusterName}";

    /// <summary>
    /// 创建用于K8S远程管理的配置文件
    /// </summary>
    public void CreateConfigFile(string clusterName, string clusterConfig)
    {
        var configFile = GetConfigFile(clusterName);
        // 文件不存在，则创建
        if (!File.Exists(configFile))
        {
            File.WriteAllText(configFile, clusterConfig);
        }
        else
        {
            // 比对配置是否不一样，不一样则覆盖新的
            var config = File.ReadAllText(configFile);
            if (clusterConfig != config)
            {
                File.WriteAllText(configFile, clusterConfig);
            }
        }
    }

    /// <summary>
    /// 更新k8s的镜像版本
    /// </summary>
    public async Task<bool> SetImages(string clusterName, string projectName, string dockerImages, EumK8sControllers k8sControllersType, IProgress<string> progress, CancellationToken cancellationToken)
    {
        var configFile = GetConfigFile(clusterName);
        var exitCode   = await ShellTools.Run("kubectl", $"set image {k8sControllersType.GetName()}/{projectName} {projectName}={dockerImages} --kubeconfig={configFile}", progress, null, null, cancellationToken);
        if (exitCode != 0)
        {
            progress.Report("K8S更新镜像失败。");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 生成yaml文件，并执行kubectl apply命令
    /// </summary>
    public async Task<bool> SetYaml(string clusterName, string projectName, string yamlContent, IProgress<string> progress, CancellationToken cancellationToken)
    {
        // 将yaml文件写入临时文件
        var fileName = $"/tmp/{projectName}.yaml";
        File.Delete(fileName);
        await File.WriteAllTextAsync(fileName, yamlContent, Encoding.UTF8, cancellationToken);

        var configFile = GetConfigFile(clusterName);

        // 发布
        var exitCode = await ShellTools.Run("kubectl", $"apply -f {fileName} --kubeconfig={configFile}", progress, null, null, cancellationToken);
        if (exitCode != 0)
        {
            progress.Report("K8S更新镜像失败。");
            return false;
        }
        return true;
    }
}