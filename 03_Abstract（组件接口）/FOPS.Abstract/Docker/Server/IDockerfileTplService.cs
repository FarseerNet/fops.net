using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Docker.Entity;
using FS.DI;

namespace FOPS.Abstract.Docker.Server
{
    public interface IDockerfileTplService: ITransientDependency
    {
        /// <summary>
        /// Dockerfile模板列表
        /// </summary>
        Task<List<DockerfileTplVO>> ToListAsync();

        /// <summary>
        /// Dockerfile模板信息
        /// </summary>
        Task<DockerfileTplVO> ToInfoAsync(int id);

        /// <summary>
        /// Dockerfile模板数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加Dockerfile模板
        /// </summary>
        Task AddAsync(DockerfileTplVO vo);

        /// <summary>
        /// 修改Dockerfile模板
        /// </summary>
        Task UpdateAsync(int id, DockerfileTplVO vo);

        /// <summary>
        /// 删除Dockerfile模板
        /// </summary>
        Task DeleteAsync(int id);
    }
}