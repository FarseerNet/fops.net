using FOPS.Domain.Sys.Admin;
using FOPS.Domain.Sys.Admin.Repository;
using FOPS.Infrastructure.Repository.Admin;
using FOPS.Infrastructure.Repository.Admin.Model;
using FS.Extends;
using Mapster;

namespace FOPS.Infrastructure.Repository;

public class AdminRepository : IAdminRepository
{
    public AdminAgent AdminAgent { get; set; }

    /// <summary>
    ///     管理员是否存在
    /// </summary>
    public Task<bool> IsExists(string adminName) => AdminAgent.IsExists(adminName);

    /// <summary>
    ///     管理员是否存在
    /// </summary>
    public Task<bool> IsExists(string adminName, int adminId) => AdminAgent.IsExists(adminName, adminId);

    /// <summary>
    ///     添加管理员
    /// </summary>
    public Task<int> AddAsync(AdminDO admin) => AdminAgent.AddAsync(admin);

    /// <summary>
    ///     修改管理员
    /// </summary>
    public Task UpdateAsync(int id, AdminDO po) => AdminAgent.UpdateAsync(id, po);

    /// <summary>
    ///     Admin列表
    /// </summary>
    public Task<List<AdminDO>> ToListAsync() => AdminAgent.ToListAsync().AdaptAsync<AdminDO, AdminPO>();

    /// <summary>
    ///     Admin信息
    /// </summary>
    public Task<AdminDO> ToInfoAsync(int id) => AdminAgent.ToInfoAsync(id).AdaptAsync<AdminDO, AdminPO>();

    /// <summary>
    ///     Admin信息
    /// </summary>
    public Task<AdminDO> ToInfoAsync(string userName, string pwd) => AdminAgent.ToInfoAsync(userName, pwd).AdaptAsync<AdminDO, AdminPO>();

    /// <summary>
    ///     Admin数量
    /// </summary>
    public Task<int> CountAsync() => AdminAgent.CountAsync();

    /// <summary>
    ///     删除管理员
    /// </summary>
    public Task DeleteAsync(int id) => AdminAgent.DeleteAsync(id);
}