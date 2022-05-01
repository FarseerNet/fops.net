using FOPS.Domain.Build.ProjectGroup;
using Mapster;

namespace FOPS.Application.Build.ProjectGroup.Entity;

public class ProjectGroupDTO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///     集群ID
    /// </summary>
    public List<int> ClusterIds { get; set; }
    /// <summary>
    ///     项目组名称
    /// </summary>
    public string Name { get; set; }

    public static implicit operator ProjectGroupDO(ProjectGroupDTO dto) => dto.Adapt<ProjectGroupDO>();
}