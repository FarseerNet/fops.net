using FOPS.Domain.Build.Git.Repository;

namespace FOPS.Domain.Build.Git;

public class GitDO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///     Git名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///     托管地址
    /// </summary>
    public string Hub { get; set; }
    /// <summary>
    ///     Git分支
    /// </summary>
    public string Branch { get; set; }
    /// <summary>
    ///     账户名称
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    ///     账户密码
    /// </summary>
    public string UserPwd { get; set; }
    /// <summary>
    ///     拉取时间
    /// </summary>
    public DateTime PullAt { get; set; }
    /// <summary>
    /// 添加GIT
    /// </summary>
    public Task<int> AddAsync()
    {
        PullAt = new DateTime(2000, 1, 1);

        var repository = IocManager.GetService<IGitRepository>();
        return repository.AddAsync(this);
    }

    /// <summary>
    /// 修改GIT
    /// </summary>
    public Task UpdateAsync()
    {
        var repository = IocManager.GetService<IGitRepository>();
        return repository.UpdateAsync(Id, this);
    }
}