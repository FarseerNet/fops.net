using FOPS.Application.Build.DockerfileTpl.Entity;
using FOPS.Domain.Build.DockerfileTpl;
using FOPS.Domain.Build.DockerfileTpl.Repository;
using FS.Extends;

namespace FOPS.Application.Build.DockerfileTpl;

public class DockerfileTplApp : ISingletonDependency
{
    public IDockerfileTplRepository DockerfileTplRepository { get; set; }

    /// <summary>
    /// 添加Dockerfile模板
    /// </summary>
    public Task AddAsync(DockerfileTplDTO dto)
    {
        DockerfileTplDO dockerfileTpl = dto;
        return dockerfileTpl.AddAsync();
    }

    /// <summary>
    /// Dockerfile模板列表
    /// </summary>
    public Task<List<DockerfileTplDTO>> ToListAsync() => DockerfileTplRepository.ToListAsync().AdaptAsync<DockerfileTplDTO, DockerfileTplDO>();
    
    /// <summary>
    /// 修改Dockerfile模板
    /// </summary>
    public Task UpdateAsync(DockerfileTplDTO dto)
    {
        DockerfileTplDO dockerfileTpl = dto;
        return dockerfileTpl.UpdateAsync();
    }
    
    /// <summary>
    /// Dockerfile模板信息
    /// </summary>
    public Task<DockerfileTplDTO> ToInfoAsync(int id) => DockerfileTplRepository.ToInfoAsync(id).AdaptAsync<DockerfileTplDTO, DockerfileTplDO>();
    
    /// <summary>
    /// Dockerfile模板数量
    /// </summary>
    public Task<int> CountAsync() => DockerfileTplRepository.CountAsync();
}