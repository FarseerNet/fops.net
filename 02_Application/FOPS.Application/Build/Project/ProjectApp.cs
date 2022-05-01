using FOPS.Application.Build.Project.Entity;
using FOPS.Domain.Build.Project;
using FOPS.Domain.Build.Project.Repository;
using FS.Extends;
using Mapster;

namespace FOPS.Application.Build.Project;

public class ProjectApp : ISingletonDependency
{
    public IProjectRepository ProjectRepository { get; set; }

    /// <summary>
    /// 项目列表
    /// </summary>
    public Task<List<ProjectDTO>> ToListAsync() => ProjectRepository.ToListAsync().AdaptAsync<ProjectDTO, ProjectDO>();

    /// <summary>
    /// 项目列表
    /// </summary>
    public Task<List<ProjectDTO>> ToListAsync(int groupId) => ProjectRepository.ToListAsync(groupId).AdaptAsync<ProjectDTO, ProjectDO>();

    /// <summary>
    /// 项目信息
    /// </summary>
    public Task<ProjectDTO> ToInfoAsync(int id) => ProjectRepository.ToInfoAsync(id).AdaptAsync<ProjectDTO, ProjectDO>();

    /// <summary>
    /// 项目信息
    /// </summary>
    public ProjectDTO ToInfo(int id) => ProjectRepository.ToInfo(id).Adapt<ProjectDTO>();

    /// <summary>
    /// 添加项目
    /// </summary>
    public Task<int> AddAsync(ProjectDTO dto)
    {
        ProjectDO project = dto;
        return project.AddAsync();
    }

    /// <summary>
    /// 修改项目
    /// </summary>
    public Task UpdateAsync(ProjectDTO dto)
    {
        ProjectDO project = dto;
        return project.UpdateAsync();
    }

    /// <summary>
    /// 使用Git的数量
    /// </summary>
    public Task<int> GitCountAsync(int id) => ProjectRepository.GitCountAsync(id);

    /// <summary>
    /// 项目数量
    /// </summary>
    public Task<int> CountAsync() => ProjectRepository.CountAsync();

    /// <summary>
    /// 应用列表
    /// </summary>
    public Task<List<ProjectDTO>> ToAppListAsync() => ProjectRepository.ToAppListAsync().AdaptAsync<ProjectDTO, ProjectDO>();

    /// <summary>
    /// 使用项目组的数量
    /// </summary>
    public Task<int> GroupCountAsync(int id) => ProjectRepository.GroupCountAsync(id);
    
    /// <summary>
    /// 删除项目
    /// </summary>
    public Task DeleteAsync(int id) => ProjectRepository.DeleteAsync(id);
}