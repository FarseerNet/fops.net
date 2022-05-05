using FOPS.Infrastructure.Repository.ContainerLog.Model;
using FOPS.Infrastructure.Repository.Context;

namespace FOPS.Infrastructure.Repository.ContainerLog;

public class ContainerLogAgent : ISingletonDependency
{
    /// <summary>
    ///     读取前500条日志
    /// </summary>
    public Task<List<ContainerLogPO>> ToListAsync(int top) => ContainerLogContext.Data.ContainerLogPO.Desc(o => o.CreateAt).ToListAsync(top);
}