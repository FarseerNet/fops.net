using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FS.DI;

namespace FOPS.Abstract.K8S.Server
{
    public interface IClusterService: ITransientDependency
    {
        /// <summary>
        /// 集群列表
        /// </summary>
        Task<List<ClusterVO>> ToListAsync();

        /// <summary>
        /// 集群信息
        /// </summary>
        Task<ClusterVO> ToInfoAsync(int id);

        /// <summary>
        /// 集群数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加集群
        /// </summary>
        Task<int> AddAsync(ClusterVO vo);

        /// <summary>
        /// 修改集群
        /// </summary>
        Task UpdateAsync(int id, ClusterVO vo);

        /// <summary>
        /// 删除集群
        /// </summary>
        Task DeleteAsync(int id);
    }
}