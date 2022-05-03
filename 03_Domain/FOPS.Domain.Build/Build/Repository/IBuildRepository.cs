namespace FOPS.Domain.Build.Build.Repository;

public interface IBuildRepository: ISingletonDependency
{
    /// <summary>
    ///     获取构建的编号
    /// </summary>
    Task<int> GetBuildNumberAsync(int projectId);
    /// <summary>
    ///     获取构建任务的主键
    /// </summary>
    Task<int> GetBuildId(int projectId, int buildNumber);
    /// <summary>
    ///     添加构建任务
    /// </summary>
    Task<int> AddAsync(BuildDO po);
    /// <summary>
    ///     主动取消任务
    /// </summary>
    Task CancelAsync(int id);
    /// <summary>
    ///     当前构建的队列数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    ///     获取构建队列前30
    /// </summary>
    Task<List<BuildDO>> ToBuildingListAsync(int pageSize, int pageIndex);
    /// <summary>
    ///     查看构建信息
    /// </summary>
    Task<BuildDO> ToInfoAsync(int id);
    /// <summary>
    /// 获取未构建的任务
    /// </summary>
    Task<BuildDO> GetUnBuildInfoAsync();
    /// <summary>
    /// 设置任务为构建中
    /// </summary>
    Task<int> SetBuilding(int buildId);
    /// <summary>
    ///     任务完成
    /// </summary>
    Task SuccessAsync(int id);
}