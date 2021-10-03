using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IProjectService: ISingletonDependency
    {
        /// <summary>
        /// 项目列表
        /// </summary>
        Task<List<ProjectVO>> ToListAsync();

        /// <summary>
        /// 项目信息
        /// </summary>
        Task<ProjectVO> ToInfoAsync(int id);

        /// <summary>
        /// 项目数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加项目
        /// </summary>
        Task<int> AddAsync(ProjectVO vo);

        /// <summary>
        /// 修改项目
        /// </summary>
        Task UpdateAsync(int id, ProjectVO vo);

        /// <summary>
        /// 删除项目
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 修改镜像版本
        /// </summary>
        Task UpdateAsync(int id, string dockerVer);

        /// <summary>
        /// 修改集群的镜像版本
        /// </summary>
        Task UpdateAsync(int id, Dictionary<int,ClusterVer> dicClusterVer);

        /// <summary>
        /// 项目信息
        /// </summary>
        ProjectVO ToInfo(int id);

        /// <summary>
        /// 项目列表
        /// </summary>
        Task<List<ProjectVO>> ToListAsync(int groupId);

        /// <summary>
        /// 应用列表
        /// </summary>
        Task<List<ProjectVO>> ToAppListAsync();
        
        /// <summary>
        /// 使用项目组的数量
        /// </summary>
        Task<int> GroupCountAsync(int groupId);
        
        /// <summary>
        /// 使用Git的数量
        /// </summary>
        Task<int> GitCountAsync(int gitId);
    }
}