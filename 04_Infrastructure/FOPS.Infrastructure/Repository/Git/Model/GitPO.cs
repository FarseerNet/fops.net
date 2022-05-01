using FOPS.Domain.Build.Git;
using FS.Core.Mapping.Attribute;
using Mapster;

namespace FOPS.Infrastructure.Repository.Git.Model;

public class GitPO
{
    /// <summary>
    ///     主键
    /// </summary>
    [Field(Name = "id", IsPrimaryKey = true, IsDbGenerated = true)]
    public int? Id { get; set; }
    /// <summary>
    ///     Git名称
    /// </summary>
    [Field(Name = "name")]
    public string Name { get; set; }
    /// <summary>
    ///     托管地址
    /// </summary>
    [Field(Name = "hub")]
    public string Hub { get; set; }
    /// <summary>
    ///     Git分支
    /// </summary>
    [Field(Name = "branch")]
    public string Branch { get; set; }
    /// <summary>
    ///     账户名称
    /// </summary>
    [Field(Name = "user_name")]
    public string UserName { get; set; }
    /// <summary>
    ///     账户密码
    /// </summary>
    [Field(Name = "user_pwd")]
    public string UserPwd { get; set; }
    /// <summary>
    ///     拉取时间
    /// </summary>
    [Field(Name = "pull_at")]
    public DateTime? PullAt { get; set; }

    public static implicit operator GitPO(GitDO git) => git.Adapt<GitPO>();
}