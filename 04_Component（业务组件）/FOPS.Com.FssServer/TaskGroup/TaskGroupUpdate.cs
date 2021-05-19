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
        public IIocManager     IocManager     { get; set; }

        /// <summary>
        /// 更新TaskGroup
        /// </summary>
        public Task UpdateAsync(TaskGroupVO vo) => TaskGroupCache.SaveAsync(vo.Id, vo);

        /// <summary>
        /// 保存TaskGroup
        /// </summary>
        public async Task SaveAsync(TaskGroupVO vo)
        {
            if (vo.IntervalMs < 1)
            {
                // 是否为数字
                if (IsType.IsInt(vo.Cron))
                {
                    vo.IntervalMs = vo.Cron.ConvertType(0L);
                    vo.Cron       = "";
                }
                else if (!new Cron().Parse(vo.Cron))
                {
                    throw new Exception("Cron格式错误");
                }
            }

            await TaskGroupAgent.UpdateAsync(vo.Id, vo.Map<TaskGroupPO>());
            await UpdateAsync(vo);
        }
    }
}