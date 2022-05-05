using FOPS.Domain.AppLog.ContainerLog.Repository;

namespace FOPS.Domain.AppLog;

public class ContainerLogService : ISingletonDependency
{
    public IContainerLogRepository ContainerLogRepository { get; set; }
}