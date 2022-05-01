using FOPS.Infrastructure.Repository.Cluster.Model;
using FOPS.Infrastructure.Repository.Context;
using FS.DI;

namespace FOPS.Infrastructure.Repository.Cluster;

public class ClusterAgent : ISingletonDependency
{
    /// <summary>
    /// 集群列表
    /// </summary>
    public Task<List<ClusterPO>> ToListAsync() => MysqlContext.Data.Cluster.Select(o => new { o.Id, o.Name, o.RuntimeEnvType }).Asc(o => o.Sort).ToListAsync();

    /// <summary>
    /// 集群信息
    /// </summary>
    public Task<ClusterPO> ToInfoAsync(int id) => MysqlContext.Data.Cluster.Where(o => o.Id == id).ToEntityAsync();

    /// <summary>
    /// 集群数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.Cluster.CountAsync();

    /// <summary>
    /// 添加集群
    /// </summary>
    public async Task<int> AddAsync(ClusterPO po)
    {
        await MysqlContext.Data.Cluster.InsertAsync(po, true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    /// 修改集群
    /// </summary>
    public Task UpdateAsync(int id, ClusterPO vo) => MysqlContext.Data.Cluster.Where(o => o.Id == id).UpdateAsync(vo);

    /// <summary>
    /// 删除集群
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.Cluster.Where(o => o.Id == id).DeleteAsync();
}