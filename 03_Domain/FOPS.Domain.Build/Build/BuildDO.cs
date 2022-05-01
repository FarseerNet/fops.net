using FOPS.Domain.Build.Build.Repository;
using FOPS.Domain.Build.Enum;
using FS;

namespace FOPS.Domain.Build.Build;

public class BuildDO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     项目ID
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    ///     集群ID
    /// </summary>
    public int ClusterId { get; set; }

    /// <summary>
    ///     构建号
    /// </summary>
    public int BuildNumber { get; set; }

    /// <summary>
    ///     状态
    /// </summary>
    public EumBuildStatus Status { get; set; }

    /// <summary>
    ///     是否成功
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    ///     开始时间
    /// </summary>
    public DateTime CreateAt { get; set; }

    /// <summary>
    ///     完成时间
    /// </summary>
    public DateTime FinishAt { get; set; }

    /// <summary>
    ///     构建的服务端id
    /// </summary>
    public string BuildServerId { get; set; }

    /// <summary>
    /// 添加构建任务
    /// </summary>
    public async Task<int> AddAsync(int projectId, int clusterId)
    {
        var repository = IocManager.GetService<IBuildRepository>();
        // 获取数据库中最后一个编译版本号
        var buildNumber = await repository.GetBuildNumberAsync(projectId);

        ProjectId     = projectId;
        ClusterId     = clusterId;
        BuildNumber   = ++buildNumber;
        Status        = EumBuildStatus.None;
        IsSuccess     = false;
        CreateAt      = DateTime.Now;
        FinishAt      = DateTime.Now;
        BuildServerId = FarseerApplication.AppId;

        return await repository.AddAsync(this);
    }
}