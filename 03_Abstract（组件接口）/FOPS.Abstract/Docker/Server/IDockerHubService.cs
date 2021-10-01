using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Docker.Entity;
using FS.DI;

namespace FOPS.Abstract.Docker.Server
{
    public interface IDockerHubService: ISingletonDependency
    {
        /// <summary>
        /// DockerHub列表
        /// </summary>
        Task<List<DockerHubVO>> ToListAsync();

        /// <summary>
        /// DockerHub信息
        /// </summary>
        Task<DockerHubVO> ToInfoAsync(int id);

        /// <summary>
        /// DockerHub数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加仓库
        /// </summary>
        Task AddAsync(DockerHubVO vo);

        /// <summary>
        /// 修改仓库
        /// </summary>
        Task UpdateAsync(int id, DockerHubVO vo);

        /// <summary>
        /// 删除仓库
        /// </summary>
        Task DeleteAsync(int id);
    }
}