using FOPS.Domain.Build.Deploy.Entity;
using FS.Core.Entity;

namespace FOPS.Domain.Build.Deploy.Device;

/// <summary>
/// 容器驱动接口
/// </summary>
public interface IDockerDevice: ISingletonDependency
{
    /// <summary>
    /// 取得dockerHub
    /// </summary>
    string GetDockerHub(string dockerHubAddress);
    /// <summary>
    /// 生成镜像名称
    /// </summary>
    string GetDockerImage(string dockerHubAddress, string projectName, int buildNumber);
    /// <summary>
    /// 登陆容器仓库
    /// </summary>
    Task<bool> LoginAsync(string dockerHub, string loginName, string loginPwd, IProgress<string> receiveOutput, BuildEnvironment env, CancellationToken cancellationToken);
    /// <summary>
    /// dockerfile文件是否存在
    /// </summary>
    bool ExistsDockerfile(string dockerfilePath);
    /// <summary>
    /// 生成Dockerfile文件
    /// </summary>
    /// <param name="projectName">dockerfile文件地址</param>
    /// <param name="dockerfileContent">文件内容</param>
    /// <param name="cancellationToken"></param>
    Task CreateDockerfileAsync(string projectName, string dockerfileContent, CancellationToken cancellationToken = default);
    /// <summary>
    /// 容器构建
    /// </summary>
    Task<bool> Build(BuildEnvironment env, IProgress<string> receiveOutput, CancellationToken cancellationToken);
    /// <summary>
    /// 上传镜像
    /// </summary>
    Task<bool> Push(BuildEnvironment env, IProgress<string> receiveOutput, CancellationToken cancellationToken);
    /// <summary>
    /// 生成Dockerfile路径
    /// </summary>
    string GetDockerfilePath(string projectName);
}