using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface IClientRegister: ISingletonDependency
    {
        /// <summary>
        /// 取出全局客户端列表
        /// </summary>
        Task<List<ClientVO>> ToClientListAsync();

        /// <summary>
        /// 取出全局客户端数量
        /// </summary>
        Task<long> ToClientCountAsync();
    }
}