using FOPS.Domain.Build.Git;
using Mapster;

namespace FOPS.Application.Build.Git.Entity;

public class GitDTO
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

    public static implicit operator GitDO(GitDTO dto) => dto.Adapt<GitDO>();
}