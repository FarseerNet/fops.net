namespace FOPS.Domain.Build.Cluster.Repository;

public interface IClusterRepository: ISingletonDependency
{
    /// <summary>
    /// 集群列表
    /// </summary>
    Task<List<ClusterDO>> ToListAsync();
    /// <summary>
    /// 集群信息
    /// </summary>
    Task<ClusterDO> ToInfoAsync(int id);
    /// <summary>
    /// 集群数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    /// 添加集群
    /// </summary>
    Task<int> AddAsync(ClusterDO cluster);
    /// <summary>
    /// 修改集群
    /// </summary>
    Task UpdateAsync(int id, ClusterDO cluster);
    /// <summary>
    /// 删除集群
    /// </summary>
    Task DeleteAsync(int id);
}