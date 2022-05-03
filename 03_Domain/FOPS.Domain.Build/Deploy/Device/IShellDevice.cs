using FOPS.Domain.Build.DeployK8S.Entity;

namespace FOPS.Domain.Build.Deploy.Device;

/// <summary>
/// Shell命令驱动接口
/// </summary>
public interface IShellDevice: ISingletonDependency
{
    /// <summary>
    /// 执行Shell脚本
    /// </summary>
    /// <returns></returns>
    Task<bool> ExecShellAsync(BuildEnvironment env, string shellScript, IProgress<string> actReceiveOutput, CancellationToken cancellationToken);
}