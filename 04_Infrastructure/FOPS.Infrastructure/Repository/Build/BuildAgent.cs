using FOPS.Domain.Build.Enum;
using FOPS.Infrastructure.Repository.Build.Model;
using FOPS.Infrastructure.Repository.Context;
using FS.DI;

namespace FOPS.Infrastructure.Repository.Build;

public class BuildAgent : ISingletonDependency
{
    /// <summary>
    /// 获取构建的编号
    /// </summary>
    public Task<int> GetBuildNumberAsync(int projectId)
    {
        return MysqlContext.Data.Build.Where(o => o.ProjectId == projectId).Desc(o => o.Id).GetValueAsync(o => o.BuildNumber.GetValueOrDefault());
    }
    
    /// <summary>
    /// 添加构建任务
    /// </summary>
    public async Task<int> AddAsync(BuildPO po)
    {
        await MysqlContext.Data.Build.InsertAsync(po, true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    /// 主动取消任务
    /// </summary>
    public Task CancelAsync(int id)
    {
        return MysqlContext.Data.Build.Where(o => o.Id == id).UpdateAsync(new BuildPO
        {
            Status    = EumBuildStatus.Finish,
            IsSuccess = false,
            FinishAt  = DateTime.Now,
        });
    }
    
    /// <summary>
    /// 获取构建任务的主键
    /// </summary>
    public int GetBuildId(int projectId, int buildNumber)
    {
        return MysqlContext.Data.Build.Where(o => o.BuildNumber == buildNumber && o.ProjectId == projectId).GetValue(o => o.Id.GetValueOrDefault());
    }

    /// <summary>
    /// 当前构建的队列数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.Build.Where(o => o.Status != EumBuildStatus.Finish).CountAsync();

    /// <summary>
    /// 获取构建队列前30
    /// </summary>
    public Task<List<BuildPO>> ToBuildingListAsync(int pageSize, int pageIndex) => MysqlContext.Data.Build.Select(o => new { o.Id, o.Status, o.BuildNumber, o.IsSuccess, o.ProjectId, o.CreateAt, o.FinishAt }).Desc(o => o.Id).ToListAsync(pageSize, pageIndex);

    /// <summary>
    /// 查看构建信息
    /// </summary>
    public Task<BuildPO> ToInfoAsync(int id) => MysqlContext.Data.Build.Where(o => o.Id == id).ToEntityAsync();
}