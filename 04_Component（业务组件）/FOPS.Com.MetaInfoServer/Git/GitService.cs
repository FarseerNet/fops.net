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
        /// 添加GIT
        /// </summary>
        public Task AddAsync(GitVO vo) => MetaInfoContext.Data.Git.InsertAsync(vo.Map<GitPO>());

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