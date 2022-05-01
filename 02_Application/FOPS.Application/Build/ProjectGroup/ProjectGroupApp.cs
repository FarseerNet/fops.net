using FOPS.Application.Build.ProjectGroup.Entity;
using FOPS.Domain.Build.ProjectGroup;
using FOPS.Domain.Build.ProjectGroup.Repository;
using FS.Extends;

namespace FOPS.Application.Build.ProjectGroup;

public class ProjectGroupApp : ISingletonDependency
{
    public IProjectGroupRepository ProjectGroupRepository { get; set; }

    /// <summary>
    /// 项目组列表
    /// </summary>
    public Task<List<ProjectGroupDTO>> ToListAsync() => ProjectGroupRepository.ToListAsync().AdaptAsync<ProjectGroupDTO, ProjectGroupDO>();
    /// <summary>
    /// 项目组信息
    /// </summary>
    public Task<ProjectGroupDTO> ToInfoAsync(int id) => ProjectGroupRepository.ToInfoAsync(id).AdaptAsync<ProjectGroupDTO, ProjectGroupDO>();

    /// <summary>
    /// 项目组数量
    /// </summary>
    public Task<int> CountAsync() => ProjectGroupRepository.CountAsync();

    /// <summary>
    /// 添加项目组
    /// </summary>
    public Task AddAsync(ProjectGroupDTO dto)
    {
        ProjectGroupDO projectGroup = dto;
        return projectGroup.AddAsync();
    }

    /// <summary>
    /// 修改项目组
    /// </summary>
    public Task UpdateAsync(ProjectGroupDTO dto)
    {
        ProjectGroupDO projectGroup = dto;
        return projectGroup.UpdateAsync();
    }

    /// <summary>
    /// 删除项目组
    /// </summary>
    public Task DeleteAsync(int id) => ProjectGroupRepository.DeleteAsync(id);
}