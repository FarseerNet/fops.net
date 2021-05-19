using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Enum;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.BuilderServer.Build.Dal;

namespace FOPS.Com.BuilderServer.Build
{
    /// <summary>
    /// 构建
    /// </summary>
    public class BuildService : IBuildService
    {
        public IGitOpr         GitOpr         { get; set; }
        public IProjectService ProjectService { get; set; }
        public IGitService     GitService     { get; set; }


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

        /// <summary>
        /// 构建
        /// </summary>
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

            // 拿到任务后，先取基本信息
            var project = await ProjectService.ToInfoAsync(po.ProjectId.GetValueOrDefault());
            if (project == null)
            {
                await Fail(po, $"项目ID={po.ProjectId.GetValueOrDefault()}，不存在");
                return;
            }

            // Git项目
            var git = await GitService.ToInfoAsync(project.GitId);
            if (git == null)
            {
                await Fail(po, $"gitID={project.GitId}，不存在");
                return;
            }

            // 1、拉取Git
            var pullResult = await GitOpr.PullAsync(git.Id);
            if (pullResult.IsError)
            {
                await Fail(po, pullResult.OutputLine);
                return;
            }

            // 2、编译
            // 3、打包
            // 4、上传镜像
            // 5、更新集群镜像版本
        }

        /// <summary>
        /// 主动取消任务
        /// </summary>
        public Task Cancel(int id)
        {
            return BuilderContext.Data.Build.Where(o => o.Id == id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = false,
                FinishAt  = DateTime.Now,
                Output    = "手动取消"
            });
        }

        /// <summary>
        /// 设置任务失败
        /// </summary>
        private Task Fail(BuildPO po, string output)
        {
            return BuilderContext.Data.Build.Where(o => o.Id == po.Id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = false,
                FinishAt  = DateTime.Now,
                Output    = output
            });
        }
    }
}