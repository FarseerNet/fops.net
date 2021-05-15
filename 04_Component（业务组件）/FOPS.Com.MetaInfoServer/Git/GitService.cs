using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.MetaInfoServer.Git.Dal;
using FS.Extends;

namespace FOPS.Com.MetaInfoServer.Git
{
    public class GitService : IGitService
    {
        /// <summary>
        /// Git列表
        /// </summary>
        public Task<List<GitVO>> ToListAsync() => MetaInfoContext.Data.Git.ToListAsync().MapAsync<GitVO, GitPO>();

        /// <summary>
        /// Git信息
        /// </summary>
        public Task<GitVO> ToInfoAsync(int id) => MetaInfoContext.Data.Git.Where(o => o.Id == id).ToEntityAsync().MapAsync<GitVO, GitPO>();

        /// <summary>
        /// Git数量
        /// </summary>
        public Task<int> CountAsync() => MetaInfoContext.Data.Git.CountAsync();

        /// <summary>
        /// 添加GIT
        /// </summary>
        public async Task<int> AddAsync(GitVO vo)
        {
            var po = vo.Map<GitPO>();
            await MetaInfoContext.Data.Git.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改GIT
        /// </summary>
        public Task UpdateAsync(int id, GitVO vo) => MetaInfoContext.Data.Git.Where(o => o.Id == id).UpdateAsync(vo.Map<GitPO>());

        /// <summary>
        /// 删除GIT
        /// </summary>
        public Task DeleteAsync(int id) => MetaInfoContext.Data.Git.Where(o => o.Id == id).DeleteAsync();
    }
}