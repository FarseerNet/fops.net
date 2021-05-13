using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface INodeClient: ITransientDependency
    {
        /// <summary>
        /// 取出全局客户端列表
        /// </summary>
        Task<List<ClientConnectVO>> ToClientListAsync();

        /// <summary>
        /// 获取服务端节点列表
        /// </summary>
        Task<Dictionary<string, DateTime>> ToNodeListAsync();
    }
}