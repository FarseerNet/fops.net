namespace FOPS.Domain.Build.ProjectGroup.Repository;

public interface IProjectGroupRepository: ISingletonDependency
{
    /// <summary>
    /// 项目组列表
    /// </summary>
    Task<List<ProjectGroupDO>> ToListAsync();
    /// <summary>
    /// 项目组信息
    /// </summary>
    Task<ProjectGroupDO> ToInfoAsync(int id);
    /// <summary>
    /// 项目组数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    /// 添加项目组
    /// </summary>
    Task<int> AddAsync(ProjectGroupDO vo);
    /// <summary>
    /// 修改项目组
    /// </summary>
    Task UpdateAsync(int id, ProjectGroupDO projectGroup);
    /// <summary>
    /// 删除项目组
    /// </summary>
    Task DeleteAsync(int id);
}