using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Docker.Entity;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.K8S.Server;
using FOPS.Com.DockerServer.DockerfileTpl.Dal;
using FS.Extends;

namespace FOPS.Com.DockerServer.DockerfileTpl
{
    public class DockerfileTplService : IDockerfileTplService
    {
        /// <summary>
        /// Dockerfile模板列表
        /// </summary>
        public Task<List<DockerfileTplVO>> ToListAsync() => DockerContext.Data.DockerfileTpl.ToListAsync().MapAsync<DockerfileTplVO, DockerfileTplPO>();

        /// <summary>
        /// Dockerfile模板信息
        /// </summary>
        public Task<DockerfileTplVO> ToInfoAsync(int id) => DockerContext.Data.DockerfileTpl.Where(o => o.Id == id).ToEntityAsync().MapAsync<DockerfileTplVO, DockerfileTplPO>();

        /// <summary>
        /// Dockerfile模板数量
        /// </summary>
        public Task<int> CountAsync() => DockerContext.Data.DockerfileTpl.CountAsync();

        /// <summary>
        /// 添加Dockerfile模板
        /// </summary>
        public async Task AddAsync(DockerfileTplVO vo) => await DockerContext.Data.DockerfileTpl.InsertAsync(vo.Map<DockerfileTplPO>());

        /// <summary>
        /// 修改Dockerfile模板
        /// </summary>
        public Task UpdateAsync(int id, DockerfileTplVO vo) => DockerContext.Data.DockerfileTpl.Where(o => o.Id == id).UpdateAsync(vo.Map<DockerfileTplPO>());

        /// <summary>
        /// 删除Dockerfile模板
        /// </summary>
        public Task DeleteAsync(int id) => DockerContext.Data.DockerfileTpl.Where(o => o.Id == id).DeleteAsync();
    }
}