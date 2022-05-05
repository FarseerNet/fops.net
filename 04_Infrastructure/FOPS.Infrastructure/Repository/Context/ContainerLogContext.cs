using FOPS.Infrastructure.Repository.ContainerLog.Model;
using FS.ElasticSearch;

namespace FOPS.Infrastructure.Repository.Context;

/// <summary>
///     容器日志上下文
/// </summary>
public class ContainerLogContext : EsContext<ContainerLogContext>
{
    static ContainerLogContext()
    {
    }

    public ContainerLogContext() : base(configName: "log_es")
    {
    }

    /// <summary>
    ///     用户索引
    /// </summary>
    public IndexSet<ContainerLogPO> ContainerLogPO { get; set; }
}