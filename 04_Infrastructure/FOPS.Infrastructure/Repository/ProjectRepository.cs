using FOPS.Domain.Build.Project;
using FOPS.Domain.Build.Project.Repository;
using FOPS.Infrastructure.Repository.Project;
using FOPS.Infrastructure.Repository.Project.Model;
using FS.Extends;
using Mapster;
using Newtonsoft.Json;

namespace FOPS.Infrastructure.Repository;

public class ProjectRepository : IProjectRepository
{
    public ProjectAgent ProjectAgent { get; set; }

    /// <summary>
    /// 项目列表
    /// </summary>
    public async Task<List<ProjectDO>> ToListAsync()
    {
        var lst = await ProjectAgent.ToListAsync().AdaptAsync<ProjectDO, ProjectPO>();
        foreach (var project in lst)
        {
            SetClusterVer(project: project);
        }
        return lst;
    }
    private void SetClusterVer(ProjectDO project)
    {
        try
        {
            project.DicClusterVer = JsonConvert.DeserializeObject<Dictionary<int, ClusterVer>>(project.ClusterVer);
        }
        catch
        {
            project.DicClusterVer = new Dictionary<int, ClusterVer>();
        }
    }

    /// <summary>
    /// 应用列表
    /// </summary>
    public Task<List<ProjectDO>> ToAppListAsync() => ProjectAgent.ToAppListAsync().AdaptAsync<ProjectDO, ProjectPO>();

    /// <summary>
    /// 项目列表
    /// </summary>
    public async Task<List<ProjectDO>> ToListAsync(int groupId)
    {
        var lst = await ProjectAgent.ToListAsync(groupId).AdaptAsync<ProjectDO, ProjectPO>();
        foreach (var project in lst)
        {
            SetClusterVer(project);
        }

        return lst;
    }

    /// <summary>
    /// 项目信息
    /// </summary>
    public async Task<ProjectDO> ToInfoAsync(int id)
    {
        var project = await ProjectAgent.ToInfoAsync(id).AdaptAsync<ProjectDO, ProjectPO>();

        SetClusterVer(project);

        return project;
    }

    /// <summary>
    /// 项目信息
    /// </summary>
    public ProjectDO ToInfo(int id)
    {
        var project = ProjectAgent.ToInfo(id).Adapt<ProjectDO>();
        if (project == null) return null;

        SetClusterVer(project);

        return project;
    }

    /// <summary>
    /// 项目数量
    /// </summary>
    public Task<int> CountAsync() => ProjectAgent.CountAsync();

    /// <summary>
    /// 使用项目组的数量
    /// </summary>
    public Task<int> GroupCountAsync(int groupId) => ProjectAgent.GroupCountAsync(groupId);

    /// <summary>
    /// 使用Git的数量
    /// </summary>
    public Task<int> GitCountAsync(int gitId) => ProjectAgent.GitCountAsync(gitId);

    /// <summary>
    /// 添加项目
    /// </summary>
    public Task<int> AddAsync(ProjectDO project) => ProjectAgent.AddAsync(project);

    /// <summary>
    /// 修改项目
    /// </summary>
    public Task UpdateAsync(int id, ProjectDO project) => ProjectAgent.UpdateAsync(id, project);

    /// <summary>
    /// 修改镜像版本
    /// </summary>
    public Task UpdateAsync(int id, string dockerVer) => ProjectAgent.UpdateAsync(id, new ProjectPO
    {
        DockerVer = dockerVer
    });

    /// <summary>
    /// 修改集群的镜像版本
    /// </summary>
    public Task UpdateAsync(int id, Dictionary<int, ClusterVer> dicClusterVer)
    {
        return ProjectAgent.UpdateAsync(id, new ProjectPO
        {
            ClusterVer = JsonConvert.SerializeObject(dicClusterVer)
        });
    }

    /// <summary>
    /// 删除项目
    /// </summary>
    public Task DeleteAsync(int id) => ProjectAgent.DeleteAsync(id);
}