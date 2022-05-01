using FOPS.Infrastructure.Repository.Context;
using FOPS.Infrastructure.Repository.DockerHub.Model;

namespace FOPS.Infrastructure.Repository.DockerHub;

public class DockerHubAgent : ISingletonDependency
{
    /// <summary>
    ///     DockerHub列表
    /// </summary>
    public Task<List<DockerHubPO>> ToListAsync() => MysqlContext.Data.DockerHub.ToListAsync();

    /// <summary>
    ///     DockerHub信息
    /// </summary>
    public Task<DockerHubPO> ToInfoAsync(int id) => MysqlContext.Data.DockerHub.Where(where: o => o.Id == id).ToEntityAsync();

    /// <summary>
    ///     DockerHub数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.DockerHub.CountAsync();

    /// <summary>
    ///     添加仓库
    /// </summary>
    public Task AddAsync(DockerHubPO po) => MysqlContext.Data.DockerHub.InsertAsync(entity: po);

    /// <summary>
    ///     修改仓库
    /// </summary>
    public Task UpdateAsync(int id, DockerHubPO po) => MysqlContext.Data.DockerHub.Where(where: o => o.Id == id).UpdateAsync(entity: po);

    /// <summary>
    ///     删除仓库
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.DockerHub.Where(where: o => o.Id == id).DeleteAsync();
}