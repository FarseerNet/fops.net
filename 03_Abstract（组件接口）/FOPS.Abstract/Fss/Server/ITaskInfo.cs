using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskInfo: ITransientDependency
    {
        /// <summary>
        /// 获取任务信息
        /// </summary>
        Task<TaskVO> ToInfoByDbAsync(int id);

        /// <summary>
        /// 今日执行失败数量
        /// </summary>
        Task<int> TodayFailCountAsync();
    }
}