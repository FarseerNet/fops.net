using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FS.DI;

namespace FOPS.Com.FssServer.Abstract
{
    public interface ITaskGroupAgent : ITransientDependency
    {
        /// <summary>
        /// 获取所有任务列表
        /// </summary>
        Task<List<TaskGroupPO>> ToListAsync();
        
        /// <summary>
        /// 获取任务信息
        /// </summary>
        Task<TaskGroupPO> ToEntityAsync(int id);

        /// <summary>
        /// 更新任务组信息
        /// </summary>
        Task UpdateAsync(int id, TaskGroupPO taskGroup);

        /// <summary>
        /// 添加任务组
        /// </summary>
        Task AddAsync(TaskGroupPO po);
    }
}