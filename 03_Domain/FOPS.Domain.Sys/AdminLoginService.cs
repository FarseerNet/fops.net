using FOPS.Domain.Sys.Admin;
using FOPS.Domain.Sys.Admin.Repository;
using FS.Utils.Common;

namespace FOPS.Domain.Sys;

public class AdminLoginService : ISingletonDependency
{
    public IAdminRepository AdminRepository { get; set; }
    
    /// <summary>
    /// 登陆
    /// </summary>
    public async Task<AdminDO> LoginAsync(string userName, string pwd, string ip)
    {
        pwd = Encrypt.MD5(pwd);
        var info = await AdminRepository.ToInfoAsync(userName, pwd);
        if (info == null) throw new Exception("用户不存在，或者密码错误");
        if (!info.IsEnable) throw new Exception("账号被冻结");

        await info.UpdateLoginAsync(ip);
        return info;
    }
}