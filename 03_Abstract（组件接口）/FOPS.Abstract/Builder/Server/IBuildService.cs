using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IBuildService: ITransientDependency
    {
        /// <summary>
        /// 创建构建任务
        /// </summary>
        Task<int> Add(int projectId, int clusterId);

        /// <summary>
        /// 构建
        /// </summary>
        Task Build();

        /// <summary>
        /// 主动取消任务
        /// </summary>
        Task Cancel(int id);

        /// <summary>
        /// 设置任务成功
        /// </summary>
        Task Success(int clusterId, ProjectVO project);

        /// <summary>
        /// 当前构建的队列数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 获取构建队列前30
        /// </summary>
        /// <returns></returns>
        Task<List<BuildVO>> ToBuildingList();
    }
}