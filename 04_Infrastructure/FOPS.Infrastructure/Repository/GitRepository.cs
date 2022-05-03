using FOPS.Domain.Build.Git;
using FOPS.Domain.Build.Git.Repository;
using FOPS.Infrastructure.Repository.Git;
using FOPS.Infrastructure.Repository.Git.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class GitRepository : IGitRepository
{
    public GitAgent GitAgent { get; set; }
    
    /// <summary>
    /// Git列表
    /// </summary>
    public Task<List<GitDO>> ToListAsync() => GitAgent.ToListAsync().AdaptAsync<GitDO, GitPO>();
    /// <summary>
    /// Git列表
    /// </summary>
    public Task<List<GitDO>> ToListAsync(List<int> ids) => GitAgent.ToListAsync(ids).AdaptAsync<GitDO, GitPO>();

    /// <summary>
    /// Git信息
    /// </summary>
    public Task<GitDO> ToInfoAsync(int id) => GitAgent.ToInfoAsync(id).AdaptAsync<GitDO, GitPO>();

    /// <summary>
    /// Git数量
    /// </summary>
    public Task<int> CountAsync() => GitAgent.CountAsync();

    /// <summary>
    /// 添加GIT
    /// </summary>
    public Task<int> AddAsync(GitDO git) => GitAgent.AddAsync(git);

    /// <summary>
    /// 修改GIT
    /// </summary>
    public Task UpdateAsync(int id, GitDO git) => GitAgent.UpdateAsync(id, git);

    /// <summary>
    /// 修改GIT的拉取时间
    /// </summary>
    public Task UpdateAsync(int id, DateTime pullAt) => GitAgent.UpdateAsync(id, new GitPO
    {
        PullAt = pullAt
    });

    /// <summary>
    /// 删除GIT
    /// </summary>
    public Task DeleteAsync(int id) => GitAgent.DeleteAsync(id);
}