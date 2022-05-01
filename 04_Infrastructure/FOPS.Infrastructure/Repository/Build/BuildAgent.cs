using FOPS.Domain.Build.Enum;
using FOPS.Infrastructure.Repository.Build.Model;
using FOPS.Infrastructure.Repository.Context;

namespace FOPS.Infrastructure.Repository.Build;

public class BuildAgent : ISingletonDependency
{
    /// <summary>
    ///     获取构建的编号
    /// </summary>
    public Task<int> GetBuildNumberAsync(int projectId)
    {
        return MysqlContext.Data.Build.Where(where: o => o.ProjectId == projectId).Desc(desc: o => o.Id).GetValueAsync(fieldName: o => o.BuildNumber.GetValueOrDefault());
    }

    /// <summary>
    ///     添加构建任务
    /// </summary>
    public async Task<int> AddAsync(BuildPO po)
    {
        await MysqlContext.Data.Build.InsertAsync(entity: po, isReturnLastId: true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    ///     主动取消任务
    /// </summary>
    public Task CancelAsync(int id)
    {
        return MysqlContext.Data.Build.Where(where: o => o.Id == id).UpdateAsync(entity: new BuildPO
        {
            Status    = EumBuildStatus.Finish,
            IsSuccess = false,
            FinishAt  = DateTime.Now
        });
    }

    /// <summary>
    ///     获取构建任务的主键
    /// </summary>
    public int GetBuildId(int projectId, int buildNumber)
    {
        return MysqlContext.Data.Build.Where(where: o => o.BuildNumber == buildNumber && o.ProjectId == projectId).GetValue(fieldName: o => o.Id.GetValueOrDefault());
    }

    /// <summary>
    ///     当前构建的队列数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.Build.Where(where: o => o.Status != EumBuildStatus.Finish).CountAsync();

    /// <summary>
    ///     获取构建队列前30
    /// </summary>
    public Task<List<BuildPO>> ToBuildingListAsync(int pageSize, int pageIndex) => MysqlContext.Data.Build.Select(@select: o => new { o.Id, o.Status, o.BuildNumber, o.IsSuccess, o.ProjectId, o.CreateAt, o.FinishAt }).Desc(desc: o => o.Id).ToListAsync(pageSize: pageSize, pageIndex: pageIndex);

    /// <summary>
    ///     查看构建信息
    /// </summary>
    public Task<BuildPO> ToInfoAsync(int id) => MysqlContext.Data.Build.Where(where: o => o.Id == id).ToEntityAsync();
}