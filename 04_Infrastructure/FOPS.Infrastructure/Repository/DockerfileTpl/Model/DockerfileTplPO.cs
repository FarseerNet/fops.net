using FOPS.Domain.Build.DockerfileTpl;
using FS.Core.Mapping.Attribute;
using Mapster;

namespace FOPS.Infrastructure.Repository.DockerfileTpl.Model;

public class DockerfileTplPO
{
    /// <summary>
    ///     主键
    /// </summary>
    [Field(Name = "id", IsPrimaryKey = true, IsDbGenerated = true)]
    public int? Id { get; set; }

    /// <summary>
    ///     模板名称
    /// </summary>
    [Field(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    ///     模板内容
    /// </summary>
    [Field(Name = "template")]
    public string Template { get; set; }

    public static implicit operator DockerfileTplPO(DockerfileTplDO dockerfileTpl) => dockerfileTpl.Adapt<DockerfileTplPO>();
}