using FOPS.Domain.AppLog.ContainerLog;
using FOPS.Domain.AppLog.ContainerLog.Repository;
using FOPS.Infrastructure.Repository.ContainerLog;
using FOPS.Infrastructure.Repository.ContainerLog.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class ContainerLogRepository : IContainerLogRepository
{
    public ContainerLogAgent ContainerLogAgent { get; set; }
    
    /// <summary>
    ///     读取前500条日志
    /// </summary>
    public Task<List<ContainerLogDO>> ToListAsync(int top) => ContainerLogAgent.ToListAsync(top).AdaptAsync<ContainerLogDO, ContainerLogPO>();
}