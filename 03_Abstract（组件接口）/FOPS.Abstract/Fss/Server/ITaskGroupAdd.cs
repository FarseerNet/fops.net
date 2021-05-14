using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskGroupAdd: ITransientDependency
    {
        /// <summary>
        /// 添加任务信息
        /// </summary>
        Task ToInfoAsync(TaskGroupVO vo);
    }
}