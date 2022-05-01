using FOPS.Infrastructure.Repository.Context;
using FOPS.Infrastructure.Repository.Git.Model;
using FS.DI;
using FS.Extends;

namespace FOPS.Infrastructure.Repository.Git;

public class GitAgent : ISingletonDependency
{
    /// <summary>
    /// Git列表
    /// </summary>
    public Task<List<GitPO>> ToListAsync() => MysqlContext.Data.Git.ToListAsync();
    /// <summary>
    /// Git列表
    /// </summary>
    public Task<List<GitPO>> ToListAsync(List<int> ids) => MysqlContext.Data.Git.Where(o => ids.Contains(o.Id)).ToListAsync();

    /// <summary>
    /// Git信息
    /// </summary>
    public Task<GitPO> ToInfoAsync(int id) => MysqlContext.Data.Git.Where(o => o.Id == id).ToEntityAsync();

    /// <summary>
    /// Git数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.Git.CountAsync();

    /// <summary>
    /// 添加GIT
    /// </summary>
    public async Task<int> AddAsync(GitPO po)
    {
        await MysqlContext.Data.Git.InsertAsync(po, true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    /// 修改GIT
    /// </summary>
    public Task UpdateAsync(int id, GitPO vo) => MysqlContext.Data.Git.Where(o => o.Id == id).UpdateAsync(vo);


    /// <summary>
    /// 修改GIT的拉取时间
    /// </summary>
    public Task UpdateAsync(int id, DateTime pullAt) => MysqlContext.Data.Git.Where(o => o.Id == id).UpdateAsync(new GitPO
    {
        PullAt = pullAt
    });

    /// <summary>
    /// 删除GIT
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.Git.Where(o => o.Id == id).DeleteAsync();
}