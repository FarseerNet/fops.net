using FOPS.Domain.Sys.Admin;
using FOPS.Domain.Sys.Admin.Repository;
using FS.Utils.Common;
using Mapster;

namespace FOPS.Application.Sys.Admin.Entity;

public class AdminDTO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///     管理员名称
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    ///     管理员密码
    /// </summary>
    public string UserPwd { get; set; }
    /// <summary>
    ///     是否启用
    /// </summary>
    public bool IsEnable { get; set; }
    /// <summary>
    ///     上次登陆时间
    /// </summary>
    public DateTime LastLoginAt { get; set; }
    /// <summary>
    ///     上次登陆IP
    /// </summary>
    public string LastLoginIp { get; set; }
    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreateAt { get; set; }
    /// <summary>
    ///     创建人
    /// </summary>
    public string CreateUser { get; set; }
    /// <summary>
    ///     创建人ID
    /// </summary>
    public string CreateId { get; set; }

    public static implicit operator AdminDO(AdminDTO admin) => admin.Adapt<AdminDO>();
}