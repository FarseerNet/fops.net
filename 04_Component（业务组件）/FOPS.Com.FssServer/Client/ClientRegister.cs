using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FS.Cache.Redis;
using FS.DI;
using FS.Extends;
using Newtonsoft.Json;

namespace FOPS.Com.FssServer.Client
{
    /// <summary>
    /// 客户端、服务端
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class ClientRegister : IClientRegister
    {
        public  IIocManager        IocManager        { get; set; }
        private IRedisCacheManager RedisCacheManager => IocManager.Resolve<IRedisCacheManager>("fss_redis");

        /// <summary>
        /// 取出全局客户端列表
        /// </summary>
        public async Task<List<ClientVO>> ToClientListAsync()
        {
            const string Key        = "FSS_ClientList";
            var          hashGetAll = await RedisCacheManager.Db.HashGetAllAsync(Key);
            if (hashGetAll == null || hashGetAll.Length == 0) return null;
            var lst = hashGetAll.Select(o => JsonConvert.DeserializeObject<ClientVO>(o.Value.ToString())).ToList();
            for (int i = 0; i < lst.Count; i++)
            {
                // 心跳大于1秒中，任为已经下线了
                if ((DateTime.Now - lst[i].ActivateAt).TotalMinutes >= 1)
                {
                    await RedisCacheManager.Db.HashDeleteAsync(Key, lst[i].Id);
                    lst.RemoveAt(i);
                    i--;
                }
            }

            return lst;
        }

        /// <summary>
        /// 取出全局客户端数量
        /// </summary>
        public Task<long> ToClientCountAsync()
        {
            const string Key = "FSS_ClientList";
            return RedisCacheManager.Db.HashLengthAsync(Key);
        }
    }
}