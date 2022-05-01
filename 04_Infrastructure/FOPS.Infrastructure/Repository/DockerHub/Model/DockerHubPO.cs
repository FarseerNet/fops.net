using FS.Core.Mapping.Attribute;

namespace FOPS.Infrastructure.Repository.DockerHub.Model;

public class DockerHubPO
{
    /// <summary>
    ///     主键
    /// </summary>
    [Field(Name = "id", IsPrimaryKey = true, IsDbGenerated = true)]
    public int? Id { get; set; }
    /// <summary>
    ///     仓库名称
    /// </summary>
    [Field(Name = "name")]
    public string Name { get; set; }
    /// <summary>
    ///     托管地址
    /// </summary>
    [Field(Name = "hub")]
    public string Hub { get; set; }
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
}