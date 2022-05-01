namespace FOPS.Domain.Build.DockerfileTpl.Repository;

public interface IDockerfileTplRepository: ISingletonDependency
{
    /// <summary>
    /// Dockerfile模板列表
    /// </summary>
    Task<List<DockerfileTplDO>> ToListAsync();
    /// <summary>
    /// Dockerfile模板信息
    /// </summary>
    Task<DockerfileTplDO> ToInfoAsync(int id);
    /// <summary>
    /// Dockerfile模板数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    /// 添加Dockerfile模板
    /// </summary>
    Task AddAsync(DockerfileTplDO dockerfileTpl);
    /// <summary>
    /// 修改Dockerfile模板
    /// </summary>
    Task UpdateAsync(int id, DockerfileTplDO dockerfileTpl);
    /// <summary>
    /// 删除Dockerfile模板
    /// </summary>
    Task DeleteAsync(int id);
}