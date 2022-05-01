using FOPS.Infrastructure.Repository.Context;
using FOPS.Infrastructure.Repository.YamlTpl.Model;
using FS.DI;

namespace FOPS.Infrastructure.Repository.YamlTpl;

public class YamlTplAgent : ISingletonDependency
{
    /// <summary>
    /// Yaml模板列表
    /// </summary>
    public Task<List<YamlTplPO>> ToListAsync() => MysqlContext.Data.YamlTpl.ToListAsync();

    /// <summary>
    /// Yaml模板信息
    /// </summary>
    public Task<YamlTplPO> ToInfoAsync(int id) => MysqlContext.Data.YamlTpl.Where(o => o.Id == id).ToEntityAsync();

    /// <summary>
    /// Yaml模板数量
    /// </summary>
    public Task<int> CountAsync() => MysqlContext.Data.YamlTpl.CountAsync();

    /// <summary>
    /// 添加Yaml模板
    /// </summary>
    public async Task<int> AddAsync(YamlTplPO po)
    {
        await MysqlContext.Data.YamlTpl.InsertAsync(po, true);
        return po.Id.GetValueOrDefault();
    }

    /// <summary>
    /// 修改Yaml模板
    /// </summary>
    public Task UpdateAsync(int id, YamlTplPO po) => MysqlContext.Data.YamlTpl.Where(o => o.Id == id).UpdateAsync(po);

    /// <summary>
    /// 删除Yaml模板
    /// </summary>
    public Task DeleteAsync(int id) => MysqlContext.Data.YamlTpl.Where(o => o.Id == id).DeleteAsync();
}