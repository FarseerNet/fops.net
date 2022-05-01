namespace FOPS.Domain.Build.YamlTpl.Repository;

public interface IYamlTplRepository: ISingletonDependency
{
    /// <summary>
    /// Yaml模板列表
    /// </summary>
    Task<List<YamlTplDO>> ToListAsync();
    /// <summary>
    /// Yaml模板信息
    /// </summary>
    Task<YamlTplDO> ToInfoAsync(int id);
    /// <summary>
    /// Yaml模板数量
    /// </summary>
    Task<int> CountAsync();
    /// <summary>
    /// 添加Yaml模板
    /// </summary>
    Task<int> AddAsync(YamlTplDO yamlTpl);
    /// <summary>
    /// 修改Yaml模板
    /// </summary>
    Task UpdateAsync(int id, YamlTplDO yamlTpl);
    /// <summary>
    /// 删除Yaml模板
    /// </summary>
    Task DeleteAsync(int id);
}