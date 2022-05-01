using FOPS.Domain.Build.YamlTpl;
using FOPS.Domain.Build.YamlTpl.Repository;
using FOPS.Infrastructure.Repository.YamlTpl;
using FOPS.Infrastructure.Repository.YamlTpl.Model;
using FS.Extends;

namespace FOPS.Infrastructure.Repository;

public class YamlTplRepository : IYamlTplRepository
{
    public YamlTplAgent YamlTplAgent { get; set; }
    /// <summary>
    /// Yaml模板列表
    /// </summary>
    public Task<List<YamlTplDO>> ToListAsync() => YamlTplAgent.ToListAsync().AdaptAsync<YamlTplDO, YamlTplPO>();

    /// <summary>
    /// Yaml模板信息
    /// </summary>
    public Task<YamlTplDO> ToInfoAsync(int id) => YamlTplAgent.ToInfoAsync(id).AdaptAsync<YamlTplDO, YamlTplPO>();

    /// <summary>
    /// Yaml模板数量
    /// </summary>
    public Task<int> CountAsync() => YamlTplAgent.CountAsync();

    /// <summary>
    /// 添加Yaml模板
    /// </summary>
    public Task<int> AddAsync(YamlTplDO yamlTpl) => YamlTplAgent.AddAsync(yamlTpl);

    /// <summary>
    /// 修改Yaml模板
    /// </summary>
    public Task UpdateAsync(int id, YamlTplDO yamlTpl) => YamlTplAgent.UpdateAsync(id, yamlTpl);

    /// <summary>
    /// 删除Yaml模板
    /// </summary>
    public Task DeleteAsync(int id) => YamlTplAgent.DeleteAsync(id);
}