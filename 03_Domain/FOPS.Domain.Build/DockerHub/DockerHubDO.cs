using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.DeployK8S.Entity;
using FOPS.Domain.Build.DockerHub.Repository;
using FS;

namespace FOPS.Domain.Build.DockerHub;

public class DockerHubDO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///     仓库名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///     托管地址
    /// </summary>
    public string Hub { get; set; }
    /// <summary>
    ///     账户名称
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    ///     账户密码
    /// </summary>
    public string UserPwd { get; set; }

    /// <summary>
    /// 添加
    /// </summary>
    public Task AddAsync()
    {
        var repository = IocManager.GetService<IDockerHubRepository>();
        return repository.AddAsync(this);
    }

    /// <summary>
    /// 修改
    /// </summary>
    public Task UpdateAsync()
    {
        var repository = IocManager.GetService<IDockerHubRepository>();
        return repository.UpdateAsync(Id, this);
    }

    /// <summary>
    /// 登陆镜像
    /// </summary>
    public Task<bool> LoginAsync(BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken = default)
    {
        progress.Report("---------------------------------------------------------");
        progress.Report("登陆镜像仓库。");

        var dockerDevice = IocManager.GetService<IDockerDevice>();
        return dockerDevice.LoginAsync(Hub, UserName, UserPwd, progress, env, cancellationToken);
    }
}