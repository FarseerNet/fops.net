using FOPS.Domain.Build.ProjectGroup.Repository;

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

    /// <summary>
    /// 添加项目组
    /// </summary>
    public Task<int> AddAsync()
    {
        var repository = IocManager.GetService<IProjectGroupRepository>();
        return repository.AddAsync(this);
    }

    /// <summary>
    /// 修改项目组
    /// </summary>
    public Task UpdateAsync()
    {
        var repository = IocManager.GetService<IProjectGroupRepository>();
        return repository.UpdateAsync(Id, this);
    }

}