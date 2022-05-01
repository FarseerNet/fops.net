using FOPS.Domain.Build.ProjectGroup;
using FS.Core.Mapping.Attribute;
using Mapster;

namespace FOPS.Infrastructure.Repository.ProjectGroup.Model;

public class ProjectGroupPO
{
    /// <summary>
    ///     主键
    /// </summary>
    [Field(Name = "id", IsPrimaryKey = true, IsDbGenerated = true)]
    public int? Id { get; set; }
    /// <summary>
    ///     集群ID
    /// </summary>
    [Field(Name = "cluster_ids", StorageType = EumStorageType.Array)]
    public List<int> ClusterIds { get; set; }
    /// <summary>
    ///     项目组名称
    /// </summary>
    [Field(Name = "name")]
    public string Name { get; set; }
    
    public static implicit operator ProjectGroupPO(ProjectGroupDO projectGroup) => projectGroup.Adapt<ProjectGroupPO>();
}