namespace FOPS.Domain.Sys.Admin.Repository;

public interface IAdminRepository: ISingletonDependency
{
    /// <summary>
    ///     管理员是否存在
    /// </summary>
    Task<bool> IsExists(string adminName);
    /// <summary>
    ///     添加管理员
    /// </summary>
    Task<int> AddAsync(AdminDO admin);
    /// <summary>
    ///     管理员是否存在
    /// </summary>
    Task<bool> IsExists(string adminName, int adminId);
    /// <summary>
    ///     修改管理员
    /// </summary>
    Task UpdateAsync(int id, AdminDO po);
    /// <summary>
    ///     Admin列表
    /// </summary>
    Task<List<AdminDO>> ToListAsync();
    /// <summary>
    ///     Admin信息
    /// </summary>
    Task<AdminDO> ToInfoAsync(int id);
    /// <summary>
    ///     Admin信息
    /// </summary>
    Task<AdminDO> ToInfoAsync(string userName, string pwd);
    /// <summary>
    ///     Admin数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    ///     删除管理员
    /// </summary>
    Task DeleteAsync(int id);
}