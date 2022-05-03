using FOPS.Domain.Build.Project.Entity;

namespace FOPS.Domain.Build.Project.Repository;

public interface IProjectRepository: ISingletonDependency
{
    /// <summary>
    /// 项目列表
    /// </summary>
    Task<List<ProjectDO>> ToListAsync();
    /// <summary>
    /// 应用列表
    /// </summary>
    Task<List<ProjectDO>> ToAppListAsync();
    /// <summary>
    /// 项目列表
    /// </summary>
    Task<List<ProjectDO>> ToListAsync(int groupId);
    /// <summary>
    /// 项目信息
    /// </summary>
    Task<ProjectDO> ToInfoAsync(int id);
    /// <summary>
    /// 项目信息
    /// </summary>
    ProjectDO ToInfo(int id);
    /// <summary>
    /// 项目数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    /// 使用项目组的数量
    /// </summary>
    Task<int> GroupCountAsync(int groupId);
    /// <summary>
    /// 使用Git的数量
    /// </summary>
    Task<int> GitCountAsync(int gitId);
    /// <summary>
    /// 添加项目
    /// </summary>
    Task<int> AddAsync(ProjectDO project);
    /// <summary>
    /// 修改项目
    /// </summary>
    Task UpdateAsync(int id, ProjectDO project);
    /// <summary>
    /// 修改镜像版本
    /// </summary>
    Task UpdateAsync(int id, string dockerVer);
    /// <summary>
    /// 修改集群的镜像版本
    /// </summary>
    Task UpdateAsync(int id, Dictionary<int, ClusterVerVO> dicClusterVer);
    /// <summary>
    /// 删除项目
    /// </summary>
    Task DeleteAsync(int id);
}