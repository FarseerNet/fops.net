using FOPS.Domain.Build.DockerfileTpl;
using FOPS.Domain.Build.DockerfileTpl.Repository;
using FOPS.Infrastructure.Repository.DockerfileTpl;
using FOPS.Infrastructure.Repository.DockerfileTpl.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class DockerfileTplRepository : IDockerfileTplRepository
{
    public DockerfileTplAgent DockerfileTplAgent { get; set; }

    /// <summary>
    /// Dockerfile模板列表
    /// </summary>
    public Task<List<DockerfileTplDO>> ToListAsync() => DockerfileTplAgent.ToListAsync().AdaptAsync<DockerfileTplDO, DockerfileTplPO>();

    /// <summary>
    /// Dockerfile模板信息
    /// </summary>
    public Task<DockerfileTplDO> ToInfoAsync(int id) => DockerfileTplAgent.ToInfoAsync(id).AdaptAsync<DockerfileTplDO, DockerfileTplPO>();

    /// <summary>
    /// Dockerfile模板数量
    /// </summary>
    public Task<int> CountAsync() => DockerfileTplAgent.CountAsync();

    /// <summary>
    /// 添加Dockerfile模板
    /// </summary>
    public Task AddAsync(DockerfileTplDO dockerfileTpl) => DockerfileTplAgent.AddAsync(dockerfileTpl);

    /// <summary>
    /// 修改Dockerfile模板
    /// </summary>
    public Task UpdateAsync(int id, DockerfileTplDO dockerfileTpl) => DockerfileTplAgent.UpdateAsync(id, dockerfileTpl);

    /// <summary>
    /// 删除Dockerfile模板
    /// </summary>
    public Task DeleteAsync(int id) => DockerfileTplAgent.DeleteAsync(id);
}