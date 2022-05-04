using FOPS.Infrastructure.Repository.Context;
using FOPS.Infrastructure.Repository.Project.Model;

namespace FOPS.Infrastructure.Repository.Project;

public class ProjectAgent : ISingletonDependency
{
    /// <summary>
    ///     项目列表
    /// </summary>
    public Task<List<ProjectPO>> ToListAsync() => MysqlContext.Data.Project.ToListAsync();

    /// <summary>
    ///     应用列表
    /// </summary>
    public Task<List<ProjectPO>> ToAppListAsync() => MysqlContext.Data.Project.Where(where: o => o.AppId != "").ToListAsync();

    /// <summary>
    ///     项目列表
    /// </summary>
    public Task<List<ProjectPO>> ToListAsync(int groupId)
    {
        return MysqlContext.Data.Project.Where(where: o => o.GroupId == groupId).Select(@select: o => new { o.Id, o.Name, o.DockerVer, o.ClusterVer, o.DockerHub, o.GroupId, o.GitId, o.BuildType,o.DockerfileTpl }).ToListAsync();
    }

    /// <summary>
    ///     项目信息
    /// </summary>
    public Task<ProjectPO> ToInfoAsync(int id) => MysqlContext.Data.Project.Where(where: o => o.Id == id).ToEntityAsync();

    /// <summary>
    ///     项目信息
    /// </summary>
    public ProjectPO ToInfo(int id) => MysqlContext.Data.Project.Where(where: o => o.Id == id).ToEntity();

    /// <summary>
    ///     项目数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.Project.CountAsync();

    /// <summary>
    ///     使用项目组的数量
    /// </summary>
    public Task<int> GroupCountAsync(int groupId) => MysqlContext.Data.Project.Where(where: o => o.GroupId == groupId).CountAsync();

    /// <summary>
    ///     使用Git的数量
    /// </summary>
    public Task<int> GitCountAsync(int gitId) => MysqlContext.Data.Project.Where(where: o => o.GitId == gitId).CountAsync();

    /// <summary>
    ///     添加项目
    /// </summary>
    public async Task<int> AddAsync(ProjectPO po)
    {
        await MysqlContext.Data.Project.InsertAsync(entity: po, isReturnLastId: true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    ///     修改项目
    /// </summary>
    public Task UpdateAsync(int id, ProjectPO po) => MysqlContext.Data.Project.Where(where: o => o.Id == id).UpdateAsync(entity: po);

    /// <summary>
    ///     删除项目
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.Project.Where(where: o => o.Id == id).DeleteAsync();
}