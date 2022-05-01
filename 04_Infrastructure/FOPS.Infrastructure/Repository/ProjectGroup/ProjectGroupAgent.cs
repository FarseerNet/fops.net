using FOPS.Infrastructure.Repository.Context;
using FOPS.Infrastructure.Repository.ProjectGroup.Model;
using FS.DI;

namespace FOPS.Infrastructure.Repository.ProjectGroup;

public class ProjectGroupAgent : ISingletonDependency
{
    /// <summary>
    /// 项目组列表
    /// </summary>
    public Task<List<ProjectGroupPO>> ToListAsync() => MysqlContext.Data.ProjectGroup.ToListAsync();

    /// <summary>
    /// 项目组信息
    /// </summary>
    public Task<ProjectGroupPO> ToInfoAsync(int id) => MysqlContext.Data.ProjectGroup.Where(o => o.Id == id).ToEntityAsync();

    /// <summary>
    /// 项目组数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.ProjectGroup.CountAsync();

    /// <summary>
    /// 添加项目组
    /// </summary>
    public async Task<int> AddAsync(ProjectGroupPO po)
    {
        await MysqlContext.Data.ProjectGroup.InsertAsync(po, true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    /// 修改项目组
    /// </summary>
    public Task UpdateAsync(int id, ProjectGroupPO vo) => MysqlContext.Data.ProjectGroup.Where(o => o.Id == id).UpdateAsync(vo);

    /// <summary>
    /// 删除项目组
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.ProjectGroup.Where(o => o.Id == id).DeleteAsync();
}