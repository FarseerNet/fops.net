using FOPS.Application.Build.Build.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Domain.Build;
using FOPS.Domain.Build.Build;
using FOPS.Domain.Build.Build.Repository;
using FOPS.Domain.Build.Project;
using FS.Extends;

namespace FOPS.Application.Build.Build;

public class BuildApp : ISingletonDependency
{
    public IBuildRepository           BuildRepository            { get; set; }
    public UpdateDockerVersionService UpdateDockerVersionService { get; set; }
    public BuildService               BuildService               { get; set; }
    /// <summary>
    /// 构建
    /// </summary>
    public Task Build() => BuildService.Build();

    /// <summary>
    /// 设置任务成功
    /// </summary>
    public async Task Success(int clusterId, ProjectDTO project, int buildId)
    {
        // 构建ID没有传的时候，通过版本号获取
        if (buildId == 0)
        {
            var buildNumber = project.DockerVer.ConvertType(0);
            buildId = await BuildRepository.GetBuildId(buildNumber, project.Id);
        }

        // 修改集群的镜像版本
        await UpdateDockerVersionService.Update(project, clusterId, buildId);
    }
    /// <summary>
    /// 主动取消任务
    /// </summary>
    public Task Cancel(int id) => BuildRepository.CancelAsync(id);

    /// <summary>
    ///     获取构建队列前30
    /// </summary>
    public Task<List<BuildDTO>> ToBuildingListAsync(int pageSize, int pageIndex) => BuildRepository.ToBuildingListAsync(pageSize: pageSize, pageIndex: pageIndex).AdaptAsync<BuildDTO, BuildDO>();

    /// <summary>
    ///     当前构建的队列数量
    /// </summary>
    public Task<int> CountAsync() => BuildRepository.CountAsync();

    /// <summary>
    /// 添加构建任务
    /// </summary>
    public Task<int> AddAsync(int projectId, int clusterId) => new BuildDO().AddAsync(projectId, clusterId);

    /// <summary>
    /// 添加构建任务
    /// </summary>
    public Task AddAsync(List<ProjectDTO> lst, int clusterId)
    {
        var lstTask = lst.Select(project => new BuildDO().AddAsync(project.Id, clusterId)).Cast<Task>().ToList();
        return Task.WhenAll(lstTask);
    }

    /// <summary>
    ///     查看构建信息
    /// </summary>
    public Task<BuildDTO> ToInfoAsync(int id) => BuildRepository.ToInfoAsync(id).AdaptAsync<BuildDTO, BuildDO>();
}