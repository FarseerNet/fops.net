using FOPS.Domain.Build.Cluster;
using FOPS.Domain.Build.Cluster.Repository;
using FOPS.Infrastructure.Repository.Cluster;
using FOPS.Infrastructure.Repository.Cluster.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class ClusterRepository : IClusterRepository
{
    public ClusterAgent ClusterAgent { get; set; }

    /// <summary>
    /// 集群列表
    /// </summary>
    public Task<List<ClusterDO>> ToListAsync() => ClusterAgent.ToListAsync().AdaptAsync<ClusterDO, ClusterPO>();

    /// <summary>
    /// 集群信息
    /// </summary>
    public Task<ClusterDO> ToInfoAsync(int id) => ClusterAgent.ToInfoAsync(id).AdaptAsync<ClusterDO, ClusterPO>();

    /// <summary>
    /// 集群数量
    /// </summary>
    public Task<int> CountAsync() => ClusterAgent.CountAsync();

    /// <summary>
    /// 添加集群
    /// </summary>
    public Task<int> AddAsync(ClusterDO cluster) => ClusterAgent.AddAsync(cluster);

    /// <summary>
    /// 修改集群
    /// </summary>
    public Task UpdateAsync(int id, ClusterDO cluster) => ClusterAgent.UpdateAsync(id, cluster);

    /// <summary>
    /// 删除集群
    /// </summary>
    public Task DeleteAsync(int id) => ClusterAgent.DeleteAsync(id);
}