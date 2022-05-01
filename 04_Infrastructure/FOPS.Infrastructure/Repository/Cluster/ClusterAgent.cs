using FOPS.Infrastructure.Repository.Cluster.Model;
using FOPS.Infrastructure.Repository.Context;

namespace FOPS.Infrastructure.Repository.Cluster;

public class ClusterAgent : ISingletonDependency
{
    /// <summary>
    ///     集群列表
    /// </summary>
    public Task<List<ClusterPO>> ToListAsync() => MysqlContext.Data.Cluster.Select(@select: o => new { o.Id, o.Name, o.RuntimeEnvType }).Asc(asc: o => o.Sort).ToListAsync();

    /// <summary>
    ///     集群信息
    /// </summary>
    public Task<ClusterPO> ToInfoAsync(int id) => MysqlContext.Data.Cluster.Where(where: o => o.Id == id).ToEntityAsync();

    /// <summary>
    ///     集群数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.Cluster.CountAsync();

    /// <summary>
    ///     添加集群
    /// </summary>
    public async Task<int> AddAsync(ClusterPO po)
    {
        await MysqlContext.Data.Cluster.InsertAsync(entity: po, isReturnLastId: true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    ///     修改集群
    /// </summary>
    public Task UpdateAsync(int id, ClusterPO vo) => MysqlContext.Data.Cluster.Where(where: o => o.Id == id).UpdateAsync(entity: vo);

    /// <summary>
    ///     删除集群
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.Cluster.Where(where: o => o.Id == id).DeleteAsync();
}