using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.MetaInfoServer.Project.Dal;
using FS.Extends;

namespace FOPS.Com.MetaInfoServer.Project
{
    public class ProjectService : IProjectService
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        public Task<List<ProjectVO>> ToListAsync() => MetaInfoContext.Data.Project.ToListAsync().MapAsync<ProjectVO, ProjectPO>();

        /// <summary>
        /// 添加项目
        /// </summary>
        public Task AddAsync(ProjectVO vo) => MetaInfoContext.Data.Project.InsertAsync(vo.Map<ProjectPO>());

        /// <summary>
        /// 修改项目
        /// </summary>
        public Task UpdateAsync(int id, ProjectVO vo) => MetaInfoContext.Data.Project.Where(o => o.Id == id).UpdateAsync(vo.Map<ProjectPO>());

        /// <summary>
        /// 删除项目
        /// </summary>
        public Task DeleteAsync(int id) => MetaInfoContext.Data.Project.Where(o => o.Id == id).DeleteAsync();
    }
}