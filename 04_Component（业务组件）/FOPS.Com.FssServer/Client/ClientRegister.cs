using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Infrastructure.Repository;
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
        /// <summary>
        /// 取出全局客户端列表
        /// </summary>
        public async Task<List<ClientVO>> ToListAsync()
        {
            var key = CacheKeys.ClientKey;
            var lst = await RedisContext.Instance.CacheManager.GetListAsync(key, () => new List<ClientVO>(), o => o.Id);

            for (int i = 0; i < lst.Count; i++)
            {
                // 心跳大于1秒中，任为已经下线了
                if ((DateTime.Now - lst[i].ActivateAt).TotalMinutes >= 1)
                {
                    await CacheKeys.ClientClear(lst[i].Id);
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
            var key = CacheKeys.ClientKey;
            return RedisContext.Instance.CacheManager.GetCountAsync(key);
        }
    }
}