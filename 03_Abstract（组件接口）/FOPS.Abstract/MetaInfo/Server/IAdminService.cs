using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IAdminService: ISingletonDependency
    {
        /// <summary>
        /// Admin列表
        /// </summary>
        Task<List<AdminVO>> ToListAsync();

        /// <summary>
        /// Admin信息
        /// </summary>
        Task<AdminVO> ToInfoAsync(int id);

        /// <summary>
        /// Admin数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加管理员
        /// </summary>
        Task<int> AddAsync(AdminVO vo);

        /// <summary>
        /// 修改管理员
        /// </summary>
        Task UpdateAsync(int id, AdminVO vo);

        /// <summary>
        /// 删除管理员
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 登陆
        /// </summary>
        Task<AdminVO> LoginAsync(string userName, string pwd, string ip);

        /// <summary>
        /// 修改密码
        /// </summary>
        Task ChangePwd(string userName, string pwd, string newPwd);
    }
}