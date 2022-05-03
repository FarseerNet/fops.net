using FOPS.Domain.Build.Enum;
using FOPS.Infrastructure.Repository.Build.Model;
using FOPS.Infrastructure.Repository.Context;
using FS;

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
    ///     修改任务
    /// </summary>
    public Task<int> UpdateAsync(int id, BuildPO po) => MysqlContext.Data.Build.Where(where: o => o.Id == id).UpdateAsync(po) ;

    /// <summary>
    ///     获取构建任务的主键
    /// </summary>
    public Task<int> GetBuildIdAsync(int projectId, int buildNumber)
    {
        return MysqlContext.Data.Build.Where(where: o => o.BuildNumber == buildNumber && o.ProjectId == projectId).GetValueAsync(fieldName: o => o.Id.GetValueOrDefault());
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

    /// <summary>
    /// 获取未构建的任务
    /// </summary>
    public Task<BuildPO> ToUnBuildInfoAsync() => MysqlContext.Data.Build.Where(o => o.Status == EumBuildStatus.None && o.BuildServerId == FarseerApplication.AppId).Asc(o => o.Id).ToEntityAsync();
}