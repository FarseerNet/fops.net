using FOPS.Domain.Sys.Admin.Repository;
using FS.Utils.Common;

namespace FOPS.Domain.Sys.Admin;

public class AdminService : ISingletonDependency
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

    /// <summary>
    /// 修改密码
    /// </summary>
    public async Task ChangePwd(string userName, string pwd, string newPwd)
    {
        if (string.IsNullOrWhiteSpace(newPwd)) throw new Exception("请输入新密码");

        var info = await AdminRepository.ToInfoAsync(userName, Encrypt.MD5(pwd));
        if (info == null) throw new Exception("原密码错误，请重新输入");
        info.UserPwd = newPwd;
        await info.UpdateAsync();
    }
}