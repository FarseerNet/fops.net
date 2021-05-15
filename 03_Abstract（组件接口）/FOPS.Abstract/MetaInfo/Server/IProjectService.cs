using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IProjectService: ITransientDependency
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        Task<List<ProjectVO>> ToListAsync();

        /// <summary>
        /// 项目信息
        /// </summary>
        Task<ProjectVO> ToInfoAsync(int id);

        /// <summary>
        /// 项目数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加项目
        /// </summary>
        Task<int> AddAsync(ProjectVO vo);

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