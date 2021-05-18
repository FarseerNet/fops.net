using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IProjectGroupService
    {
        /// <summary>
        /// 项目组列表
        /// </summary>
        Task<List<ProjectGroupVO>> ToListAsync();

        /// <summary>
        /// 项目组信息
        /// </summary>
        Task<ProjectGroupVO> ToInfoAsync(int id);

        /// <summary>
        /// 项目组数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加项目组
        /// </summary>
        Task<int> AddAsync(ProjectGroupVO vo);

        /// <summary>
        /// 修改项目组
        /// </summary>
        Task UpdateAsync(int id, ProjectGroupVO vo);

        /// <summary>
        /// 删除项目组
        /// </summary>
        Task DeleteAsync(int id);
    }
}