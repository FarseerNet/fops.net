using FOPS.Domain.Build.DeployK8S.Entity;

namespace FOPS.Domain.Build.Deploy.Device;

/// <summary>
/// dotnet命令驱动接口
/// </summary>
public interface IDotnetDevice: ISingletonDependency
{
    /// <summary>
    /// 获取编译保存的目录地址
    /// </summary>
    string GetReleasePath(string projectName);
    /// <summary>
    /// 检查项目文件是否存在
    /// </summary>
    bool CheckExistsSource(BuildEnvironment env, IProgress<string> receiveOutput);
    /// <summary>
    /// 编译.net core
    /// </summary>
    Task<bool> Publish(string savePath, string source, IProgress<string> actReceiveOutput, CancellationToken cancellationToken);
    /// <summary>
    /// 获取项目源地址
    /// </summary>
    string GetSourceDirRoot(string github, string projectPath);
    /// <summary>
    /// 编译.net core
    /// </summary>
    Task<bool> Publish(BuildEnvironment env, IProgress<string> actReceiveOutput, CancellationToken cancellationToken);
}