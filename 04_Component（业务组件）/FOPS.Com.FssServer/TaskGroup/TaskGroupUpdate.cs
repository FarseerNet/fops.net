using System;
using System.Threading.Tasks;
using FOPS.Abstract.Fss.Entity;
using FOPS.Abstract.Fss.Server;
using FOPS.Com.FssServer.Abstract;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FS.DI;
using FS.Extends;
using FS.Utils.Common;
using FS.Utils.Component;

namespace FOPS.Com.FssServer.TaskGroup
{
    public class TaskGroupUpdate : ITaskGroupUpdate
    {
        public ITaskGroupCache TaskGroupCache { get; set; }
        public ITaskGroupAgent TaskGroupAgent { get; set; }

        /// <summary>
        /// 更新TaskGroup
        /// </summary>
        public Task UpdateAsync(TaskGroupVO vo) => TaskGroupCache.SaveAsync(vo.Id, vo);

        /// <summary>
        /// 保存TaskGroup
        /// </summary>
        public async Task SaveAsync(TaskGroupVO vo)
        {
            await TaskGroupAgent.UpdateAsync(vo.Id, vo.Map<TaskGroupPO>());
            await UpdateAsync(vo);
        }
    }
}