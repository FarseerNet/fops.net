using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.MetaInfoServer.Admin.Dal;
using FS.Extends;
using FS.Utils.Common;

namespace FOPS.Com.MetaInfoServer.Admin
{
    public class AdminService : IAdminService
    {
        /// <summary>
        /// Admin列表
        /// </summary>
        public Task<List<AdminVO>> ToListAsync() => MetaInfoContext.Data.Admin.ToListAsync().MapAsync<AdminVO, AdminPO>();

        /// <summary>
        /// Admin信息
        /// </summary>
        public Task<AdminVO> ToInfoAsync(int id) => MetaInfoContext.Data.Admin.Where(o => o.Id == id).ToEntityAsync().MapAsync<AdminVO, AdminPO>();

        /// <summary>
        /// Admin数量
        /// </summary>
        public Task<int> CountAsync() => MetaInfoContext.Data.Admin.CountAsync();

        /// <summary>
        /// 添加管理员
        /// </summary>
        public async Task<int> AddAsync(AdminVO vo)
        {
            vo.UserPwd = Encrypt.MD5(vo.UserPwd);
            var po = vo.Map<AdminPO>();
            await MetaInfoContext.Data.Admin.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改管理员
        /// </summary>
        public Task UpdateAsync(int id, AdminVO vo)
        {
            vo.UserPwd = vo.UserPwd == "" ? null : Encrypt.MD5(vo.UserPwd);
            return MetaInfoContext.Data.Admin.Where(o => o.Id == id).UpdateAsync(vo.Map<AdminPO>());
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        public Task DeleteAsync(int id) => MetaInfoContext.Data.Admin.Where(o => o.Id == id).DeleteAsync();

        /// <summary>
        /// 登陆
        /// </summary>
        public async Task<AdminVO> LoginAsync(string userName, string pwd)
        {
            pwd = Encrypt.MD5(pwd);
            var info = await MetaInfoContext.Data.Admin.Where(o =>o.UserName == userName && o.UserPwd == pwd).ToEntityAsync();
            if (info == null) throw new Exception("用户不存在，或者密码错误");
            if (!info.IsEnable.GetValueOrDefault())throw new Exception("账号被冻结");
            return info.Map<AdminVO>();
        }
    }
}