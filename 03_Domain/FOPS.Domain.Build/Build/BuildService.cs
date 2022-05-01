using FOPS.Domain.Build.Build.Repository;

namespace FOPS.Domain.Build.Build;

public class BuildService : ISingletonDependency
{
    public IBuildRepository BuildRepository { get; set; }
    
    /// <summary>
    /// 当前构建的队列数量
    /// </summary>
    public Task<int> CountAsync() => BuildRepository.CountAsync();

    /// <summary>
    /// 获取构建队列前30
    /// </summary>
    /// <returns></returns>
    public Task<List<BuildDO>> ToBuildingListAsync(int pageSize, int pageIndex) => BuildRepository.ToBuildingListAsync(pageSize, pageIndex);

    /// <summary>
    /// 查看构建信息
    /// </summary>
    public Task<BuildDO> ToInfoAsync(int id) => BuildRepository.ToInfoAsync(id);
}