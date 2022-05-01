using FOPS.Domain.Build.DockerfileTpl.Repository;

namespace FOPS.Domain.Build.DockerfileTpl;

public class DockerfileTplDO
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

    /// <summary>
    /// 添加Dockerfile模板
    /// </summary>
    public Task AddAsync()
    {
        var repository = IocManager.GetService<IDockerfileTplRepository>();
        return repository.AddAsync(this);
    }

    /// <summary>
    /// 修改Dockerfile模板
    /// </summary>
    public Task UpdateAsync()
    {
        var repository = IocManager.GetService<IDockerfileTplRepository>();
        return repository.UpdateAsync(Id, this);
    }
}