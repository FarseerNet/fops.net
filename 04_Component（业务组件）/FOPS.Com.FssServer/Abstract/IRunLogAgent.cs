using System.Threading.Tasks;
using FOPS.Com.FssServer.RunLog.Dal;
using FS.DI;

namespace FOPS.Com.FssServer.Abstract
{
    public interface IRunLogAgent : ITransientDependency
    {
        /// <summary>
        /// 添加日志记录
        /// </summary>
        Task AddAsync(RunLogPO po);
    }
}