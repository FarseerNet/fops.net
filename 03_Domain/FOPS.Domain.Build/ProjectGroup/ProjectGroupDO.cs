namespace FOPS.Domain.Build.ProjectGroup;

public class ProjectGroupDO
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
}