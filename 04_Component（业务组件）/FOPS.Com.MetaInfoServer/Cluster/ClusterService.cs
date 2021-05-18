using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.K8S.Server;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.K8sServerAA.Cluster.Dal;
using FS.Extends;

namespace FOPS.Com.MetaInfoServer.Cluster
{
    public class ClusterService : IClusterService
    {
        /// <summary>
        /// 集群列表
        /// </summary>
        public Task<List<ClusterVO>> ToListAsync() => MetaInfoContext.Data.Cluster.ToListAsync().MapAsync<ClusterVO, ClusterPO>();

        /// <summary>
        /// 集群信息
        /// </summary>
        public Task<ClusterVO> ToInfoAsync(int id) => MetaInfoContext.Data.Cluster.Where(o => o.Id == id).ToEntityAsync().MapAsync<ClusterVO, ClusterPO>();

        /// <summary>
        /// 集群数量
        /// </summary>
        public Task<int> CountAsync() => MetaInfoContext.Data.Cluster.CountAsync();

        /// <summary>
        /// 添加集群
        /// </summary>
        public async Task<int> AddAsync(ClusterVO vo)
        {
            var po = vo.Map<ClusterPO>();
            await MetaInfoContext.Data.Cluster.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改集群
        /// </summary>
        public Task UpdateAsync(int id, ClusterVO vo) => MetaInfoContext.Data.Cluster.Where(o => o.Id == id).UpdateAsync(vo.Map<ClusterPO>());

        /// <summary>
        /// 删除集群
        /// </summary>
        public Task DeleteAsync(int id) => MetaInfoContext.Data.Cluster.Where(o => o.Id == id).DeleteAsync();
    }
}