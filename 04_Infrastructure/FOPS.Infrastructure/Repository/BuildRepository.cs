using FOPS.Domain.Build.Build;
using FOPS.Domain.Build.Build.Repository;
using FOPS.Domain.Build.Enum;
using FOPS.Infrastructure.Repository.Build;
using FOPS.Infrastructure.Repository.Build.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class BuildRepository : IBuildRepository
{
    public BuildAgent BuildAgent { get; set; }

    /// <summary>
    ///     获取构建的编号
    /// </summary>
    public Task<int> GetBuildNumberAsync(int projectId) => BuildAgent.GetBuildNumberAsync(projectId);

    /// <summary>
    ///     获取构建任务的主键
    /// </summary>
    public Task<int> GetBuildId(int projectId, int buildNumber) => BuildAgent.GetBuildIdAsync(projectId, buildNumber);

    /// <summary>
    ///     添加构建任务
    /// </summary>
    public Task<int> AddAsync(BuildDO po) => BuildAgent.AddAsync(po);

    /// <summary>
    ///     主动取消任务
    /// </summary>
    public Task CancelAsync(int id) => BuildAgent.UpdateAsync(id, new BuildPO
    {
        Status    = EumBuildStatus.Finish,
        IsSuccess = false,
        FinishAt  = DateTime.Now
    });

    /// <summary>
    ///     当前构建的队列数量
    /// </summary>
    public Task<int> CountAsync() => BuildAgent.CountAsync();

    /// <summary>
    ///     获取构建队列前30
    /// </summary>
    public Task<List<BuildDO>> ToBuildingListAsync(int pageSize, int pageIndex) => BuildAgent.ToBuildingListAsync(pageSize: pageSize, pageIndex: pageIndex).AdaptAsync<BuildDO, BuildPO>();

    /// <summary>
    ///     查看构建信息
    /// </summary>
    public Task<BuildDO> ToInfoAsync(int id) => BuildAgent.ToInfoAsync(id).AdaptAsync<BuildDO, BuildPO>();
}