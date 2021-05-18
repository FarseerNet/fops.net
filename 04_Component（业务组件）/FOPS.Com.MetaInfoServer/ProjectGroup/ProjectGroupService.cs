using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.MetaInfoServer.ProjectGroup.Dal;
using FS.Extends;

namespace FOPS.Com.MetaInfoServer.ProjectGroup
{
    public class ProjectGroupService : IProjectGroupService
    {
        /// <summary>
        /// 项目组列表
        /// </summary>
        public Task<List<ProjectGroupVO>> ToListAsync() => MetaInfoContext.Data.ProjectGroup.ToListAsync().MapAsync<ProjectGroupVO, ProjectGroupPO>();

        /// <summary>
        /// 项目组信息
        /// </summary>
        public Task<ProjectGroupVO> ToInfoAsync(int id) => MetaInfoContext.Data.ProjectGroup.Where(o => o.Id == id).ToEntityAsync().MapAsync<ProjectGroupVO, ProjectGroupPO>();

        /// <summary>
        /// 项目组数量
        /// </summary>
        public Task<int> CountAsync() => MetaInfoContext.Data.ProjectGroup.CountAsync();

        /// <summary>
        /// 添加项目组
        /// </summary>
        public async Task<int> AddAsync(ProjectGroupVO vo)
        {
            var po = vo.Map<ProjectGroupPO>();
            await MetaInfoContext.Data.ProjectGroup.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改项目组
        /// </summary>
        public Task UpdateAsync(int id, ProjectGroupVO vo) => MetaInfoContext.Data.ProjectGroup.Where(o => o.Id == id).UpdateAsync(vo.Map<ProjectGroupPO>());

        /// <summary>
        /// 删除项目组
        /// </summary>
        public Task DeleteAsync(int id) => MetaInfoContext.Data.ProjectGroup.Where(o => o.Id == id).DeleteAsync();
    }
}