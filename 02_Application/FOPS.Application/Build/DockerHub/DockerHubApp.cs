using FOPS.Application.Build.DockerHub.Entity;
using FOPS.Domain.Build.DockerHub;
using FOPS.Domain.Build.DockerHub.Repository;
using FS.Extends;

namespace FOPS.Application.Build.DockerHub;

public class DockerHubApp : ISingletonDependency
{
    public IDockerHubRepository DockerHubRepository { get; set; }

    /// <summary>
    /// 添加
    /// </summary>
    public Task AddAsync(DockerHubDTO dto)
    {
        DockerHubDO dockerHub = dto;
        return dockerHub.AddAsync();
    }

    /// <summary>
    /// DockerHub列表
    /// </summary>
    public Task<List<DockerHubDTO>> ToListAsync() => DockerHubRepository.ToListAsync().AdaptAsync<DockerHubDTO, DockerHubDO>();

    /// <summary>
    /// 修改
    /// </summary>
    public Task UpdateAsync(DockerHubDTO dto)
    {
        DockerHubDO dockerHub = dto;
        return dockerHub.UpdateAsync();
    }

    /// <summary>
    /// DockerHub信息
    /// </summary>
    public Task<DockerHubDTO> ToInfoAsync(int id) => DockerHubRepository.ToInfoAsync(id).AdaptAsync<DockerHubDTO, DockerHubDO>();
    
    /// <summary>
    /// DockerHub数量
    /// </summary>
    public Task<int> CountAsync() => DockerHubRepository.CountAsync();
}