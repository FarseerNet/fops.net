using FOPS.Domain.Build.DeployK8S.Entity;

namespace FOPS.Domain.Build.Deploy.Device;

/// <summary>
/// Git命令驱动接口
/// </summary>
public interface IGitDevice : ISingletonDependency
{
    /// <summary>
    /// 获取项目GIT源代码存的位置/var/lib/fops/git/{gitName}/
    /// </summary>
    string GetGitPath(string gitHub);
    /// <summary>
    /// 记住密码
    /// </summary>
    Task RememberPassword(BuildEnvironment env, IProgress<string> actReceiveOutput, CancellationToken cancellationToken);
    /// <summary>
    /// 克隆
    /// </summary>
    Task<bool> Clone(string github, string branch, IProgress<string> actReceiveOutput, CancellationToken cancellationToken);
    /// <summary>
    /// 拉取最新代码
    /// </summary>
    Task<bool> Pull(string savePath, IProgress<string> actReceiveOutput, CancellationToken cancellationToken);
    /// <summary>
    /// 项目GIT是否存在
    /// </summary>
    bool ExistsGitProject(string gitPath);
    /// <summary>
    /// 消除仓库
    /// </summary>
    Task<bool> ClearAsync(string github, IProgress<string> actReceiveOutput);
}