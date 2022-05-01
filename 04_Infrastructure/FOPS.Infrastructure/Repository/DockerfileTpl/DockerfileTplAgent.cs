using FOPS.Infrastructure.Repository.Context;
using FOPS.Infrastructure.Repository.DockerfileTpl.Model;

namespace FOPS.Infrastructure.Repository.DockerfileTpl;

public class DockerfileTplAgent : ISingletonDependency
{
    /// <summary>
    ///     Dockerfile模板列表
    /// </summary>
    public Task<List<DockerfileTplPO>> ToListAsync() => MysqlContext.Data.DockerfileTpl.ToListAsync();

    /// <summary>
    ///     Dockerfile模板信息
    /// </summary>
    public Task<DockerfileTplPO> ToInfoAsync(int id) => MysqlContext.Data.DockerfileTpl.Where(where: o => o.Id == id).ToEntityAsync();

    /// <summary>
    ///     Dockerfile模板数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.DockerfileTpl.CountAsync();

    /// <summary>
    ///     添加Dockerfile模板
    /// </summary>
    public async Task AddAsync(DockerfileTplPO vo) => await MysqlContext.Data.DockerfileTpl.InsertAsync(entity: vo);

    /// <summary>
    ///     修改Dockerfile模板
    /// </summary>
    public Task UpdateAsync(int id, DockerfileTplPO vo) => MysqlContext.Data.DockerfileTpl.Where(where: o => o.Id == id).UpdateAsync(entity: vo);

    /// <summary>
    ///     删除Dockerfile模板
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.DockerfileTpl.Where(where: o => o.Id == id).DeleteAsync();
}