using FOPS.Domain.Build.Cluster.Repository;
using FOPS.Domain.Build.Enum;

namespace FOPS.Domain.Build.Cluster;

public class ClusterDO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///     集群名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///     本地kubectl配置地址
    /// </summary>
    public string Config { get; set; }
    /// <summary>
    ///     排序（越小越前）
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    ///     集群环境类型
    /// </summary>
    public EumRuntimeEnv RuntimeEnvType { get; set; }

    /// <summary>
    /// 添加集群
    /// </summary>
    public Task<int> AddAsync()
    {
        var repository = IocManager.GetService<IClusterRepository>();
        return repository.AddAsync(this);
    }

    /// <summary>
    /// 修改集群
    /// </summary>
    public Task UpdateAsync()
    {
        var repository = IocManager.GetService<IClusterRepository>();
        return repository.UpdateAsync(Id, this);
    }
}