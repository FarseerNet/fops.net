using FOPS.Domain.Build.DeployK8S.Entity;
using FS.Core.Entity;

namespace FOPS.Domain.Build.DeployK8s;

// ReSharper disable once InconsistentNaming
public class DeployK8SDO
{
    /// <summary>
    ///     项目名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///     程序入口名称
    /// </summary>
    public string EntryPoint { get; set; }
    /// <summary>
    ///     程序启动端口
    /// </summary>
    public int EntryPort { get; set; }
    /// <summary>
    ///     访问域名
    /// </summary>
    public string Domain { get; set; }
    /// <summary>
    /// 集群环境
    /// </summary>
    public ClusterVO Cluster { get; set; }
    /// <summary>
    ///     K8SDeployment模板
    /// </summary>
    public string K8STplDeployment { get; set; }
    /// <summary>
    ///     K8SIngress模板
    /// </summary>
    public string K8STplIngress { get; set; }
    /// <summary>
    ///     K8SService模板
    /// </summary>
    public string K8STplService { get; set; }
    /// <summary>
    ///     K8SConfig模板
    /// </summary>
    public string K8STplConfig { get; set; }
    /// <summary>
    ///     K8S模板自定义变量(K1=V1,K2=V2)
    /// </summary>
    public string K8STplVariable { get; set; }
}