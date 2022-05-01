using FOPS.Domain.Build.DockerHub;
using FOPS.Domain.Build.DockerHub.Repository;
using FOPS.Infrastructure.Repository.DockerHub;
using FOPS.Infrastructure.Repository.DockerHub.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class DockerHubRepository : IDockerHubRepository
{
    public DockerHubAgent DockerHubAgent { get; set; }
    
    /// <summary>
    /// DockerHub列表
    /// </summary>
    public Task<List<DockerHubDO>> ToListAsync() => DockerHubAgent.ToListAsync().AdaptAsync<DockerHubDO, DockerHubPO>();

    /// <summary>
    /// DockerHub信息
    /// </summary>
    public Task<DockerHubDO> ToInfoAsync(int id) => DockerHubAgent.ToInfoAsync(id).AdaptAsync<DockerHubDO, DockerHubPO>();

    /// <summary>
    /// DockerHub数量
    /// </summary>
    public Task<int> CountAsync() => DockerHubAgent.CountAsync();

    /// <summary>
    /// 添加仓库
    /// </summary>
    public async Task AddAsync(DockerHubDO dockerHub) => await DockerHubAgent.AddAsync(dockerHub);

    /// <summary>
    /// 修改仓库
    /// </summary>
    public Task UpdateAsync(int id, DockerHubDO dockerHub) => DockerHubAgent.UpdateAsync(id, dockerHub);

    /// <summary>
    /// 删除仓库
    /// </summary>
    public Task DeleteAsync(int id) => DockerHubAgent.DeleteAsync(id);
}