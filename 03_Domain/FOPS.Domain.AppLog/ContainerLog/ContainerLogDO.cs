using Microsoft.Extensions.Logging;

namespace FOPS.Domain.AppLog.ContainerLog;

/// <summary>
/// 容器日志记录
/// </summary>
public class ContainerLogDO
{
    /// <summary>
    ///     主键
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///     应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    ///     容器名称
    /// </summary>
    public string ContainerName { get; set; }

    /// <summary>
    ///     镜像名称
    /// </summary>
    public string ContainerImage { get; set; }

    /// <summary>
    ///     容器IP
    /// </summary>
    public string ContainerIp { get; set; }

    /// <summary>
    /// 环境变量
    /// </summary>
    public Dictionary<string, string> ContainerEnv { get; set; }
    
    /// <summary>
    ///     节点名称
    /// </summary>
    public string NodeName { get; set; }

    /// <summary>
    ///     节点IP
    /// </summary>
    public string NodeIp { get; set; }
    
    /// <summary>
    ///     日志级别
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    ///     日志内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    ///     日志时间
    /// </summary>
    public DateTime CreateAt { get; set; }
}