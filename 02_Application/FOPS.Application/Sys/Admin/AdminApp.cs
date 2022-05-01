using FOPS.Application.Sys.Admin.Entity;
using FOPS.Domain.Sys.Admin;
using FOPS.Domain.Sys.Admin.Repository;
using FS.Extends;

namespace FOPS.Application.Sys.Admin;

public class AdminApp : ISingletonDependency
{
    public IAdminRepository AdminRepository { get; set; }

    /// <summary>
    /// 添加管理员
    /// </summary>
    public Task AddAsync(AdminDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.UserPwd))
        {
            throw new Exception("管理员名称、登陆密码必须填写。");
        }

        AdminDO admin = dto;
        return admin.AddAsync();
    }

    /// <summary>
    ///     Admin列表
    /// </summary>
    public Task<List<AdminDTO>> ToListAsync() => AdminRepository.ToListAsync().AdaptAsync<AdminDTO, AdminDO>();

    /// <summary>
    ///     Admin信息
    /// </summary>
    public async Task<AdminDTO> ToInfoAsync(int id)
    {
        var dto                      = await AdminRepository.ToInfoAsync(id).AdaptAsync<AdminDTO, AdminDO>();
        if (dto != null) dto.UserPwd = null;
        return dto;
    }

    /// <summary>
    ///     修改管理员
    /// </summary>
    public Task UpdateAsync(AdminDTO dto)
    {
        if (dto == null || dto.Id < 1) throw new Exception("管理员不存在。");
        if (string.IsNullOrWhiteSpace(dto.UserName)) throw new Exception("管理员名称必须填写。");

        AdminDO admin = dto;

        return admin.UpdateAsync();
    }
}