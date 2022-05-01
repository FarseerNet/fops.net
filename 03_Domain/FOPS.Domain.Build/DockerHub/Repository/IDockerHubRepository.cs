namespace FOPS.Domain.Build.DockerHub.Repository;

public interface IDockerHubRepository: ISingletonDependency
{
    /// <summary>
    /// DockerHub列表
    /// </summary>
    Task<List<DockerHubDO>> ToListAsync();
    /// <summary>
    /// DockerHub信息
    /// </summary>
    Task<DockerHubDO> ToInfoAsync(int id);
    /// <summary>
    /// DockerHub数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    /// 添加仓库
    /// </summary>
    Task AddAsync(DockerHubDO dockerHub);
    /// <summary>
    /// 修改仓库
    /// </summary>
    Task UpdateAsync(int id, DockerHubDO dockerHub);
    /// <summary>
    /// 删除仓库
    /// </summary>
    Task DeleteAsync(int id);
}