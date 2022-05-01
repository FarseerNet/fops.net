using FOPS.Application.Build.YamlTpl.Entity;
using FOPS.Domain.Build.YamlTpl;
using FOPS.Domain.Build.YamlTpl.Repository;
using FS.Extends;

namespace FOPS.Application.Build.YamlTpl;

public class YamlTplApp : ISingletonDependency
{
    public IYamlTplRepository YamlTplRepository { get; set; }

    /// <summary>
    /// Yaml模板列表
    /// </summary>
    public Task<List<YamlTplDTO>> ToListAsync() => YamlTplRepository.ToListAsync().AdaptAsync<YamlTplDTO, YamlTplDO>();

    /// <summary>
    /// 添加Yaml模板
    /// </summary>
    public Task AddAsync(YamlTplDTO dto)
    {
        YamlTplDO yamlTpl = dto;
        return yamlTpl.AddAsync();
    }

    /// <summary>
    /// 修改Yaml模板
    /// </summary>
    public Task UpdateAsync(YamlTplDTO dto)
    {
        YamlTplDO yamlTpl = dto;
        return yamlTpl.UpdateAsync();
    }

    /// <summary>
    /// Yaml模板信息
    /// </summary>
    public Task<YamlTplDTO> ToInfoAsync(int id) => YamlTplRepository.ToInfoAsync(id).AdaptAsync<YamlTplDTO, YamlTplDO>();
    
    /// <summary>
    /// Yaml模板数量
    /// </summary>
    public Task<int> CountAsync() => YamlTplRepository.CountAsync();
}