using FOPS.Infrastructure.Repository.Admin.Model;
using FOPS.Infrastructure.Repository.Context;
using FS.DI;

namespace FOPS.Infrastructure.Repository.Admin;

public class AdminAgent : ISingletonDependency
{
    /// <summary>
    /// Admin列表
    /// </summary>
    public Task<List<AdminPO>> ToListAsync() => MysqlContext.Data.Admin.ToListAsync();

    /// <summary>
    /// Admin信息
    /// </summary>
    public Task<AdminPO> ToInfoAsync(int id) => MysqlContext.Data.Admin.Where(o => o.Id == id).ToEntityAsync();

    /// <summary>
    /// Admin信息
    /// </summary>
    public Task<AdminPO> ToInfoAsync(string userName, string pwd)
    {
        return MysqlContext.Data.Admin.Where(o => o.UserName == userName && o.UserPwd == pwd).ToEntityAsync();
    }

    /// <summary>
    /// Admin数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.Admin.CountAsync();

    /// <summary>
    /// 管理员是否存在
    /// </summary>
    public Task<bool> IsExists(string adminName) => MysqlContext.Data.Admin.Where(o => o.UserName == adminName).IsHavingAsync();

    /// <summary>
    /// 管理员是否存在
    /// </summary>
    public Task<bool> IsExists(string adminName, int adminId) => MysqlContext.Data.Admin.Where(o => o.UserName == adminName && o.Id != adminId).IsHavingAsync();

    /// <summary>
    /// 添加管理员
    /// </summary>
    public async Task<int> AddAsync(AdminPO po)
    {
        await MysqlContext.Data.Admin.InsertAsync(po, true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    /// 修改管理员
    /// </summary>
    public async Task UpdateAsync(int id, AdminPO po) => await MysqlContext.Data.Admin.Where(o => o.Id == id).UpdateAsync(po);

    /// <summary>
    /// 删除管理员
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.Admin.Where(o => o.Id == id).DeleteAsync();
}