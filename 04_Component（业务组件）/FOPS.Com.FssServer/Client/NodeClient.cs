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
    public class NodeClient : INodeClient
    {
        public  IIocManager        IocManager        { get; set; }
        private IRedisCacheManager RedisCacheManager => IocManager.Resolve<IRedisCacheManager>("fss_redis");

        /// <summary>
        /// 取出全局客户端列表
        /// </summary>
        public async Task<List<ClientConnectVO>> ToClientListAsync()
        {
            const string Key        = "FSS_ClientList";
            var          hashGetAll = await RedisCacheManager.Db.HashGetAllAsync(Key);
            if (hashGetAll == null || hashGetAll.Length == 0) return null;
            var lst = hashGetAll.Select(o => JsonConvert.DeserializeObject<ClientConnectVO>(o.Value.ToString())).ToList();
            for (int i = 0; i < lst.Count; i++)
            {
                // 心跳大于1秒中，任为已经下线了
                if ((DateTime.Now - lst[i].HeartbeatAt).TotalMinutes >= 1)
                {
                    RedisCacheManager.Db.HashDelete(Key, lst[i].ServerHost);
                    lst.RemoveAt(i);
                    i--;
                }
            }

            return lst;
        }

        /// <summary>
        /// 获取服务端节点列表
        /// </summary>
        public async Task<Dictionary<string, DateTime>> ToNodeListAsync()
        {
            const string Key        = "FSS_NodeList";
            var          hashGetAll = await RedisCacheManager.Db.HashGetAllAsync(Key);
            if (hashGetAll == null || hashGetAll.Length == 0) return null;
            var dic = new Dictionary<string, DateTime>();
            foreach (var hashEntry in hashGetAll)
            {
                var time = hashEntry.Value.ToString().ConvertType(0L).ToTimestamp();
                if ((DateTime.Now - time).TotalSeconds > 30) continue;
                dic[hashEntry.Name.ToString()] = time;
            }

            return dic;
        }
    }
}