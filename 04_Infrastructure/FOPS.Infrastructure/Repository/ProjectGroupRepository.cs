using FOPS.Domain.Build.ProjectGroup;
using FOPS.Domain.Build.ProjectGroup.Repository;
using FOPS.Infrastructure.Repository.ProjectGroup;
using FOPS.Infrastructure.Repository.ProjectGroup.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class ProjectGroupRepository : IProjectGroupRepository
{
    public ProjectGroupAgent ProjectGroupAgent { get; set; }
    
    /// <summary>
    /// 项目组列表
    /// </summary>
    public Task<List<ProjectGroupDO>> ToListAsync() => ProjectGroupAgent.ToListAsync().AdaptAsync<ProjectGroupDO, ProjectGroupPO>();

    /// <summary>
    /// 项目组信息
    /// </summary>
    public Task<ProjectGroupDO> ToInfoAsync(int id) => ProjectGroupAgent.ToInfoAsync(id).AdaptAsync<ProjectGroupDO, ProjectGroupPO>();

    /// <summary>
    /// 项目组数量
    /// </summary>
    public Task<int> CountAsync() => ProjectGroupAgent.CountAsync();

    /// <summary>
    /// 添加项目组
    /// </summary>
    public Task<int> AddAsync(ProjectGroupDO vo) => ProjectGroupAgent.AddAsync(vo);

    /// <summary>
    /// 修改项目组
    /// </summary>
    public Task UpdateAsync(int id, ProjectGroupDO projectGroup) => ProjectGroupAgent.UpdateAsync(id,projectGroup);

    /// <summary>
    /// 删除项目组
    /// </summary>
    public Task DeleteAsync(int id) => ProjectGroupAgent.DeleteAsync(id);
}