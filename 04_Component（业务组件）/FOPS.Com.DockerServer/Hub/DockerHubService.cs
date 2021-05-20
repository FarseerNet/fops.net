using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Docker.Entity;
using FOPS.Abstract.Docker.Server;
using FOPS.Com.DockerServer.Hub.Dal;
using FS.Extends;

namespace FOPS.Com.DockerServer.Hub
{
    public class DockerHubService : IDockerHubService
    {
        /// <summary>
        /// DockerHub列表
        /// </summary>
        public Task<List<DockerHubVO>> ToListAsync() => DockerContext.Data.DockerHub.ToListAsync().MapAsync<DockerHubVO, DockerHubPO>();

        /// <summary>
        /// DockerHub信息
        /// </summary>
        public Task<DockerHubVO> ToInfoAsync(int id) => DockerContext.Data.DockerHub.Where(o => o.Id == id).ToEntityAsync().MapAsync<DockerHubVO, DockerHubPO>();

        /// <summary>
        /// DockerHub数量
        /// </summary>
        public Task<int> CountAsync() => DockerContext.Data.DockerHub.CountAsync();

        /// <summary>
        /// 添加仓库
        /// </summary>
        public async Task AddAsync(DockerHubVO vo)
        {
            await DockerContext.Data.DockerHub.InsertAsync(vo.Map<DockerHubPO>());
        }

        /// <summary>
        /// 修改仓库
        /// </summary>
        public Task UpdateAsync(int id, DockerHubVO vo) => DockerContext.Data.DockerHub.Where(o => o.Id == id).UpdateAsync(vo.Map<DockerHubPO>());


        /// <summary>
        /// 删除仓库
        /// </summary>
        public Task DeleteAsync(int id) => DockerContext.Data.DockerHub.Where(o => o.Id == id).DeleteAsync();
    }
}