using FOPS.Domain.Build.Enum;
using FOPS.Domain.Build.YamlTpl;
using Mapster;

namespace FOPS.Application.Build.YamlTpl.Entity;

public class YamlTplDTO
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

    public static implicit operator YamlTplDO(YamlTplDTO dto) => dto.Adapt<YamlTplDO>();
}