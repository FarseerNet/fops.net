using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskGroupInfo: ITransientDependency
    {
        /// <summary>
        /// 获取任务信息
        /// </summary>
        Task<TaskGroupVO> ToInfoAsync(int id);

        /// <summary>
        /// 从数据库中取出并保存
        /// </summary>
        Task<TaskGroupVO> ToInfoByDbAsync(int id);
    }
}