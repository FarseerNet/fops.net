using FOPS.Domain.AppLog.ContainerLog;
using Mapster;
using Nest;

namespace FOPS.Infrastructure.Repository.ContainerLog.Model;

[ElasticsearchType(IdProperty = "Id")]
public class ContainerLogPO
{
    /// <summary>
    ///     主键
    /// </summary>
    [Keyword]
    public string Id { get; set; }

    /// <summary>
    ///     应用名称
    /// </summary>
    [Keyword]
    public string AppName { get; set; }

    /// <summary>
    ///     容器名称
    /// </summary>
    [Keyword]
    public string ContainerName { get; set; }

    /// <summary>
    ///     镜像名称
    /// </summary>
    [Keyword]
    public string ContainerImage { get; set; }

    /// <summary>
    ///     容器IP
    /// </summary>
    [Keyword]
    public string ContainerIp { get; set; }

    /// <summary>
    /// 环境变量
    /// </summary>
    [Flattened]
    public Dictionary<string, string> ContainerEnv { get; set; }

    /// <summary>
    ///     节点名称
    /// </summary>
    [Keyword]
    public string NodeName { get; set; }

    /// <summary>
    ///     节点IP
    /// </summary>
    [Keyword]
    public string NodeIp { get; set; }

    /// <summary>
    ///     日志级别
    /// </summary>
    [Keyword]
    public string LogLevel { get; set; }

    /// <summary>
    ///     日志内容
    /// </summary>
    [Text]
    public string Content { get; set; }

    /// <summary>
    ///     日志时间
    /// </summary>
    [Date]
    public long CreateAt { get; set; }

    public static implicit operator ContainerLogPO(ContainerLogDO log) => log.Adapt<ContainerLogPO>();
}