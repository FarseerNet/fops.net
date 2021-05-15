using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IProjectService
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        Task<List<ProjectVO>> ToListAsync();

        /// <summary>
        /// 添加项目
        /// </summary>
        Task AddAsync(ProjectVO vo);

        /// <summary>
        /// 修改项目
        /// </summary>
        Task UpdateAsync(int id, ProjectVO vo);

        /// <summary>
        /// 删除项目
        /// </summary>
        Task DeleteAsync(int id);
    }
}