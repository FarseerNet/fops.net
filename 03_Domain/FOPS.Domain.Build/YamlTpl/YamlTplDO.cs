using FOPS.Domain.Build.Enum;
using FOPS.Domain.Build.YamlTpl.Repository;

namespace FOPS.Domain.Build.YamlTpl;

public class YamlTplDO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     k8s类型
    /// </summary>
    public EumK8SKind K8SKindType { get; set; }

    /// <summary>
    ///     模板名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     模板内容
    /// </summary>
    public string Template { get; set; }

    /// <summary>
    /// 添加Yaml模板
    /// </summary>
    public Task<int> AddAsync()
    {
        var repository = IocManager.GetService<IYamlTplRepository>();
        return repository.AddAsync(this);
    }

    /// <summary>
    /// 修改Yaml模板
    /// </summary>
    public Task UpdateAsync()
    {
        var repository = IocManager.GetService<IYamlTplRepository>();
        return repository.UpdateAsync(Id, this);
    }
}