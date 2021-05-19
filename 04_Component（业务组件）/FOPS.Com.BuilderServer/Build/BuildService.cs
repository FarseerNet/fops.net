using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Enum;
using FOPS.Abstract.Builder.Server;
using FOPS.Com.BuilderServer.Build.Dal;

namespace FOPS.Com.BuilderServer.Build
{
    public class BuildService : IBuildService
    {
        /// <summary>
        /// 创建构建任务
        /// </summary>
        public async Task<int> Add(int projectId)
        {
            var isHave = await BuilderContext.Data.Build.Where(o => o.ProjectId == projectId && (o.Status == EumBuildStatus.None || o.Status == EumBuildStatus.Building)).IsHavingAsync();
            if (isHave) throw new Exception("当前队列存在未完成的构建");

            // 获取数据库中最后一个编译版本号
            var buildNumber = await BuilderContext.Data.Build.Where(o => o.ProjectId == projectId).Desc(o => o.Id).GetValueAsync(o => o.BuildNumber.GetValueOrDefault());
            var po = new BuildPO
            {
                ProjectId   = projectId,
                BuildNumber = ++buildNumber,
                Status      = EumBuildStatus.None,
                IsSuccess   = false,
                CreateAt    = DateTime.Now,
                UseTime     = 0,
                FinishAt    = DateTime.Now,
                Output      = ""
            };

            await BuilderContext.Data.Build.InsertAsync(po);
            return buildNumber;
        }

        public async Task Build()
        {
            // 取出未开始的任务
            var po = await BuilderContext.Data.Build.Where(o => o.Status == EumBuildStatus.None).Asc(o => o.Id).ToEntityAsync();
            if (po == null) return;

            // 设置为构建中
            po.Status = EumBuildStatus.Building;
            var isUpdate = await BuilderContext.Data.Build.Where(o => o.Id == po.Id && o.Status == EumBuildStatus.None).UpdateAsync(po) > 0;
            // 没有更新成功，说明已经被抢了
            if (!isUpdate) return;
            
        }
    }
}