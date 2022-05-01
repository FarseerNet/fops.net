using FOPS.Domain.Build.Enum;
using FS.Core.Mapping.Attribute;

namespace FOPS.Infrastructure.Repository.Cluster.Model;

public class ClusterPO
{
    /// <summary>
    ///     主键
    /// </summary>
    [Field(Name = "id", IsPrimaryKey = true, IsDbGenerated = true)]
    public int? Id { get; set; }
    /// <summary>
    ///     集群名称
    /// </summary>
    [Field(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    ///     本地kubectl配置地址
    /// </summary>
    [Field(Name = "config")]
    public string Config { get; set; }

    /// <summary>
    ///     排序（越小越前）
    /// </summary>
    [Field(Name = "sort")]
    public int? Sort { get; set; }

    /// <summary>
    ///     集群环境类型
    /// </summary>
    [Field(Name = "runtime_env_type")]
    public EumRuntimeEnv? RuntimeEnvType { get; set; }
}