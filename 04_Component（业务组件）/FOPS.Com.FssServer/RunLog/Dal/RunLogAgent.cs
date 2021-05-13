using System.Threading.Tasks;
using FOPS.Com.FssServer.Abstract;

namespace FOPS.Com.FssServer.RunLog.Dal
{
    /// <summary>
    /// 日志记录数据库层
    /// </summary>
    public class RunLogAgent : IRunLogAgent
    {
        /// <summary>
        /// 添加日志记录
        /// </summary>
        public Task AddAsync(RunLogPO po) => FssContext.Data.RunLog.InsertAsync(po);

    }
}