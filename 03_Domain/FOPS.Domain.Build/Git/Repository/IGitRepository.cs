namespace FOPS.Domain.Build.Git.Repository;

public interface IGitRepository: ISingletonDependency
{
    /// <summary>
    /// Git列表
    /// </summary>
    Task<List<GitDO>> ToListAsync();
    /// <summary>
    /// Git列表
    /// </summary>
    Task<List<GitDO>> ToListAsync(List<int> ids);
    /// <summary>
    /// Git信息
    /// </summary>
    Task<GitDO> ToInfoAsync(int id);
    /// <summary>
    /// Git数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    /// 添加GIT
    /// </summary>
    Task<int> AddAsync(GitDO git);
    /// <summary>
    /// 修改GIT
    /// </summary>
    Task UpdateAsync(int id, GitDO git);
    /// <summary>
    /// 修改GIT的拉取时间
    /// </summary>
    Task UpdateAsync(int id, DateTime pullAt);
    /// <summary>
    /// 删除GIT
    /// </summary>
    Task DeleteAsync(int id);
}