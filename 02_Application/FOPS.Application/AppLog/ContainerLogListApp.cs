using FOPS.Application.AppLog.ContainerLog.Entity;
using FOPS.Domain.AppLog.ContainerLog;
using FOPS.Domain.AppLog.ContainerLog.Repository;
using FS.Extends;

namespace FOPS.Application.AppLog;

public class ContainerLogListApp : ISingletonDependency
{
    public IContainerLogRepository ContainerLogRepository { get; set; }
    
    /// <summary>
    ///     读取前500条日志
    /// </summary>
    public async Task<List<ContainerLogDTO>> ToListAsync()
    {
        var lst =await ContainerLogRepository.ToListAsync(100).AdaptAsync<ContainerLogDTO, ContainerLogDO>();
        return lst.OrderBy(o => o.CreateAt).ToList();
    }
}