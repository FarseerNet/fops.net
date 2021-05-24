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
        public async Task<AdminVO> ToInfoAsync(int id)
        {
            var info                       = await MetaInfoContext.Data.Admin.Where(o => o.Id == id).ToEntityAsync().MapAsync<AdminVO, AdminPO>();
            if (info != null) info.UserPwd = null;
            return info;
        }

        /// <summary>
        /// Admin数量
        /// </summary>
        public Task<int> CountAsync() => MetaInfoContext.Data.Admin.CountAsync();

        /// <summary>
        /// 添加管理员
        /// </summary>
        public async Task<int> AddAsync(AdminVO vo)
        {
            var isExists = await MetaInfoContext.Data.Admin.Where(o => o.UserName == vo.UserName).IsHavingAsync();
            if (isExists) throw new Exception("管理员名称已存在");
            if (vo.UserPwd.Length < 6) throw new Exception("管理员密码长度不能小于6");
            vo.UserPwd  = Encrypt.MD5(vo.UserPwd);
            vo.CreateAt = DateTime.Now;
            var po = vo.Map<AdminPO>();
            await MetaInfoContext.Data.Admin.InsertAsync(po, true);
            vo.Id = po.Id.GetValueOrDefault();
            return vo.Id;
        }

        /// <summary>
        /// 修改管理员
        /// </summary>
        public async Task UpdateAsync(int id, AdminVO vo)
        {
            var isExists = await MetaInfoContext.Data.Admin.Where(o => o.UserName == vo.UserName && o.Id != id).IsHavingAsync();
            if (isExists) throw new Exception("管理员名称已存在");
            vo.UserPwd = vo.UserPwd == "" ? null : Encrypt.MD5(vo.UserPwd);
            if (vo.UserPwd is {Length: < 6}) throw new Exception("管理员密码长度不能小于6");
            await MetaInfoContext.Data.Admin.Where(o => o.Id == id).UpdateAsync(new AdminPO
            {
                UserName = vo.UserName,
                UserPwd  = vo.UserPwd,
                IsEnable = vo.IsEnable
            });
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        public Task DeleteAsync(int id) => MetaInfoContext.Data.Admin.Where(o => o.Id == id).DeleteAsync();

        /// <summary>
        /// 登陆
        /// </summary>
        public async Task<AdminVO> LoginAsync(string userName, string pwd, string ip)
        {
            pwd = Encrypt.MD5(pwd);
            var info = await MetaInfoContext.Data.Admin.Where(o => o.UserName == userName && o.UserPwd == pwd).ToEntityAsync();
            if (info == null) throw new Exception("用户不存在，或者密码错误");
            if (!info.IsEnable.GetValueOrDefault()) throw new Exception("账号被冻结");
            await MetaInfoContext.Data.Admin.Where(o => o.UserName == userName).UpdateAsync(new AdminPO
            {
                LastLoginAt = DateTime.Now,
                LastLoginIp = ip
            });
            return info.Map<AdminVO>();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public async Task ChangePwd(string userName, string pwd, string newPwd)
        {
            pwd = Encrypt.MD5(pwd);
            if (string.IsNullOrWhiteSpace(newPwd)) throw new Exception("请输入新密码");
            newPwd = Encrypt.MD5(newPwd);

            var info = await MetaInfoContext.Data.Admin.Where(o => o.UserName == userName && o.UserPwd == pwd).ToEntityAsync();
            if (info == null) throw new Exception("原密码错误，请重新输入");
            await MetaInfoContext.Data.Admin.Where(o => o.UserName == userName).UpdateAsync(new AdminPO
            {
                UserPwd = newPwd
            });
        }
    }
}