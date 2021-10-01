using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IGitService: ISingletonDependency
    {
        /// <summary>
        /// Git列表
        /// </summary>
        Task<List<GitVO>> ToListAsync();

        /// <summary>
        /// Git信息
        /// </summary>
        Task<GitVO> ToInfoAsync(int id);

        /// <summary>
        /// Git数量
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// 添加GIT
        /// </summary>
        Task<int> AddAsync(GitVO vo);

        /// <summary>
        /// 修改GIT
        /// </summary>
        Task UpdateAsync(int id, GitVO vo);

        /// <summary>
        /// 删除GIT
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 修改GIT的拉取时间
        /// </summary>
        Task UpdateAsync(int id, DateTime pullAt);
    }
}