namespace FOPS.Domain.AppLog.ContainerLog.Repository;

public interface IContainerLogRepository: ISingletonDependency
{
    /// <summary>
    ///     读取前500条日志
    /// </summary>
    Task<List<ContainerLogDO>> ToListAsync(int top);
}