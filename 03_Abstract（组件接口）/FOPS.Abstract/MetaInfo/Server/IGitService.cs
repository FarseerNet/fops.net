using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;

namespace FOPS.Abstract.MetaInfo.Server
{
    public interface IGitService
    {
        /// <summary>
        /// Git列表
        /// </summary>
        Task<List<GitVO>> ToListAsync();

        /// <summary>
        /// 添加GIT
        /// </summary>
        Task AddAsync(GitVO vo);

        /// <summary>
        /// 修改GIT
        /// </summary>
        Task UpdateAsync(int id, GitVO vo);

        /// <summary>
        /// 删除GIT
        /// </summary>
        Task DeleteAsync(int id);
    }
}