using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.K8SServer.Cluster.Dal;
using FS.Extends;

namespace FOPS.Com.K8SServer.Cluster
{
    public class ClusterService : IClusterService
    {
        /// <summary>
        /// 集群列表
        /// </summary>
        public Task<List<ClusterVO>> ToListAsync() => K8SContext.Data.Cluster.ToListAsync().MapAsync<ClusterVO, ClusterPO>();

        /// <summary>
        /// 集群信息
        /// </summary>
        public Task<ClusterVO> ToInfoAsync(int id) => K8SContext.Data.Cluster.Where(o => o.Id == id).ToEntityAsync().MapAsync<ClusterVO, ClusterPO>();

        /// <summary>
        /// 集群数量
        /// </summary>
        public Task<int> CountAsync() => K8SContext.Data.Cluster.CountAsync();

        /// <summary>
        /// 添加集群
        /// </summary>
        public async Task<int> AddAsync(ClusterVO vo)
        {
            var po = vo.Map<ClusterPO>();
            await K8SContext.Data.Cluster.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改集群
        /// </summary>
        public Task UpdateAsync(int id, ClusterVO vo) => K8SContext.Data.Cluster.Where(o => o.Id == id).UpdateAsync(vo.Map<ClusterPO>());

        /// <summary>
        /// 删除集群
        /// </summary>
        public Task DeleteAsync(int id) => K8SContext.Data.Cluster.Where(o => o.Id == id).DeleteAsync();
    }
}