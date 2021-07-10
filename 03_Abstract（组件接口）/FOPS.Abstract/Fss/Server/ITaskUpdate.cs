using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FS.DI;

namespace FOPS.Abstract.Fss.Server
{
    public interface ITaskUpdate: ITransientDependency
    {
        /// <summary>
        /// 任务组修改时，需要同步JobName
        /// </summary>
        Task UpdateJobName(int id, string jobName);

        /// <summary>
        /// 保存Task（taskGroup必须是最新的）
        /// </summary>
        Task SaveFinishAsync(TaskVO task, TaskGroupVO taskGroup);
    }
}