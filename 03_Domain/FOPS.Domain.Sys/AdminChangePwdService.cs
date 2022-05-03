using FOPS.Domain.Sys.Admin.Repository;
using FS.Utils.Common;

namespace FOPS.Domain.Sys;

public class AdminChangePwdService : ISingletonDependency
{
    public IAdminRepository AdminRepository { get; set; }
    
    /// <summary>
    /// 修改密码
    /// </summary>
    public async Task Change(string userName, string pwd, string newPwd)
    {
        if (string.IsNullOrWhiteSpace(newPwd)) throw new Exception("请输入新密码");

        var info = await AdminRepository.ToInfoAsync(userName, Encrypt.MD5(pwd));
        if (info == null) throw new Exception("原密码错误，请重新输入");
        info.UserPwd = newPwd;
        await info.UpdateAsync();
    }
}