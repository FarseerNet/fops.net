using FOPS.Domain.Sys.Admin.Repository;
using FS.Utils.Common;

namespace FOPS.Domain.Sys.Admin;

public class AdminDO
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

    /// <summary>
    /// 添加管理员
    /// </summary>
    public async Task<int> AddAsync()
    {
        if (UserPwd.Length < 6) throw new Exception("管理员密码长度不能小于6");
        UserPwd     = Encrypt.MD5(UserPwd);
        CreateAt    = DateTime.Now;
        LastLoginIp = "";

        var adminRepository = IocManager.GetService<IAdminRepository>();
        var isExists        = await adminRepository.IsExists(UserName);
        if (isExists) throw new Exception("管理员名称已存在");

        return await adminRepository.AddAsync(this);
    }

    /// <summary>
    /// 修改管理员
    /// </summary>
    public async Task UpdateAsync()
    {
        UserPwd = string.IsNullOrWhiteSpace(UserPwd) ? null : Encrypt.MD5(UserPwd);
        if (UserPwd is { Length: < 6 }) throw new Exception("管理员密码长度不能小于6");

        var adminRepository = IocManager.GetService<IAdminRepository>();
        var isExists        = await adminRepository.IsExists(UserName, Id);
        if (isExists) throw new Exception("管理员名称已存在");

        await adminRepository.UpdateAsync(Id, this);
    }

    /// <summary>
    /// 登陆成功后，修改登陆时间信息
    /// </summary>
    public async Task UpdateLoginAsync(string ip)
    {
        var adminRepository = IocManager.GetService<IAdminRepository>();

        LastLoginAt = DateTime.Now;
        LastLoginIp = ip;
        await adminRepository.UpdateAsync(Id, this);
    }
}