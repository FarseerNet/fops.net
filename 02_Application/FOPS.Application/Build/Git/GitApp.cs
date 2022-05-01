using FOPS.Application.Build.Git.Entity;
using FOPS.Domain.Build.Git;
using FOPS.Domain.Build.Git.Repository;
using FS.Extends;

namespace FOPS.Application.Build.Git;

public class GitApp : ISingletonDependency
{
    public IGitRepository GitRepository { get; set; }

    /// <summary>
    /// Git列表
    /// </summary>
    public Task<List<GitDTO>> ToListAsync() => GitRepository.ToListAsync().AdaptAsync<GitDTO, GitDO>();

    /// <summary>
    /// 添加GIT
    /// </summary>
    public Task AddAsync(GitDTO dto)
    {
        GitDO git = dto;
        return git.AddAsync();
    }

    /// <summary>
    /// 修改GIT的拉取时间
    /// </summary>
    public Task UpdateAsync(int gitId, DateTime time) => GitRepository.UpdateAsync(gitId, time);

    /// <summary>
    /// 修改GIT
    /// </summary>
    public Task UpdateAsync(GitDTO dto)
    {
        GitDO git = dto;
        return git.UpdateAsync();
    }

    /// <summary>
    /// Git信息
    /// </summary>
    public Task<GitDTO> ToInfoAsync(int id) => GitRepository.ToInfoAsync(id).AdaptAsync<GitDTO, GitDO>();
    
    /// <summary>
    /// 删除GIT
    /// </summary>
    public Task DeleteAsync(int id) => GitRepository.DeleteAsync(id);
    
    /// <summary>
    /// Git数量
    /// </summary>
    public Task<int> CountAsync() => GitRepository.CountAsync();
}