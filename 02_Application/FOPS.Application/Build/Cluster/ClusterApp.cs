using FOPS.Application.Build.Cluster.Entity;
using FOPS.Domain.Build.Cluster;
using FOPS.Domain.Build.Cluster.Repository;
using FS.Extends;

namespace FOPS.Application.Build.Cluster;

public class ClusterApp : ISingletonDependency
{
    public IClusterRepository ClusterRepository { get; set; }

    /// <summary>
    /// 集群列表
    /// </summary>
    public Task<List<ClusterDTO>> ToListAsync() => ClusterRepository.ToListAsync().AdaptAsync<ClusterDTO, ClusterDO>();

    /// <summary>
    /// 添加集群
    /// </summary>
    public Task AddAsync(ClusterDTO dto)
    {
        ClusterDO cluster = dto;
        return cluster.AddAsync();
    }

    /// <summary>
    /// 修改集群
    /// </summary>
    public Task UpdateAsync(ClusterDTO dto)
    {
        ClusterDO cluster = dto;
        return cluster.UpdateAsync();
    }

    /// <summary>
    /// 集群信息
    /// </summary>
    public Task<ClusterDTO> ToInfoAsync(int id) => ClusterRepository.ToInfoAsync(id).AdaptAsync<ClusterDTO, ClusterDO>();
    
    /// <summary>
    /// 集群数量
    /// </summary>
    public Task<int> CountAsync() => ClusterRepository.CountAsync();
}