using System.Threading.Tasks;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskUpdate: ITransientDependency
    {
        /// <summary>
        /// 任务组修改时，需要同步JobName
        /// </summary>
        Task UpdateJobName(int id, string jobName);
    }
}