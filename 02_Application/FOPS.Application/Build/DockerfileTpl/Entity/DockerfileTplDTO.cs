using FOPS.Domain.Build.DockerfileTpl;
using Mapster;

namespace FOPS.Application.Build.DockerfileTpl.Entity;

public class DockerfileTplDTO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///     模板名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///     模板内容
    /// </summary>
    public string Template { get; set; }

    public static implicit operator DockerfileTplDO(DockerfileTplDTO dto) => dto.Adapt<DockerfileTplDO>();
}