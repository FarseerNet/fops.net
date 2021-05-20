using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Enum;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.BuilderServer.Build.Dal;
using FS.DI;
using FS.Extends;
using Microsoft.Extensions.Logging;

namespace FOPS.Com.BuilderServer.Build
{
    /// <summary>
    /// 构建
    /// </summary>
    public class BuildService : IBuildService
    {
        public IGitOpr          GitOpr          { get; set; }
        public IDotnetOpr       DotnetOpr       { get; set; }
        public IProjectService  ProjectService  { get; set; }
        public IGitService      GitService      { get; set; }
        public IBuildLogService BuildLogService { get; set; }
        public IIocManager      IocManager      { get; set; }

        /// <summary>
        /// 构建
        /// </summary>
        public async Task Build()
        {
            // 取出未开始的任务
            var po = await BuilderContext.Data.Build.Where(o => o.Status == EumBuildStatus.None).Asc(o => o.Id).ToEntityAsync();
            if (po == null) return;
            var build = po.Map<BuildVO>();

            // 设置为构建中
            var isUpdate = await BuilderContext.Data.Build.Where(o => o.Id == build.Id && o.Status == EumBuildStatus.None).UpdateAsync(new BuildPO
            {
                Status = EumBuildStatus.Building
            }) > 0;

            // 没有更新成功，说明已经被抢了
            if (!isUpdate) return;

            // 拿到任务后，先取基本信息
            BuildLogService.Write(build.Id, $"找到构建任务：{build.Id}");
            var project = await ProjectService.ToInfoAsync(build.ProjectId);
            if (project == null)
            {
                await Fail(build, 0);
                BuildLogService.Write(build.Id, $"项目ID={build.ProjectId}，不存在");
                return;
            }

            // Git项目
            var git = await GitService.ToInfoAsync(project.GitId);
            if (git == null)
            {
                await Fail(build, 0);
                BuildLogService.Write(build.Id, $"gitID={project.GitId}，不存在");
                return;
            }

            var startNew = Stopwatch.StartNew();

            void ActWriteLog(string output)
            {
                BuildLogService.Write(build.Id, output);
            }

            // 1、拉取Git
            BuildLogService.Write(build.Id, $"构建任务id={build.Id}：开始拉取git。");
            var pullResult = await GitOpr.PullAsync(build, project, git, ActWriteLog);

            // Git拉取完成
            BuildLogService.Write(build.Id, $"构建任务id={build.Id}：拉取完成，Error={pullResult.IsError}");
            if (pullResult.IsError)
            {
                await Fail(build, startNew.ElapsedMilliseconds);
                return;
            }

            // 2、更新git拉取时间
            await GitService.UpdateAsync(git.Id, DateTime.Now);

            // 3、编译
            await DotnetOpr.Publish(build, project, git, ActWriteLog);

            // 4、打包
            // 5、上传镜像
            // 6、更新集群镜像版本
            await Success(build, startNew.ElapsedMilliseconds);
        }

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
            };

            await BuilderContext.Data.Build.InsertAsync(po);
            return buildNumber;
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
            });
        }

        /// <summary>
        /// 设置任务失败
        /// </summary>
        private Task Fail(BuildVO po, long useTime)
        {
            return BuilderContext.Data.Build.Where(o => o.Id == po.Id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = false,
                FinishAt  = DateTime.Now,
                UseTime   = useTime
            });
        }

        /// <summary>
        /// 设置任务成功
        /// </summary>
        private Task Success(BuildVO po, long useTime)
        {
            return BuilderContext.Data.Build.Where(o => o.Id == po.Id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = true,
                FinishAt  = DateTime.Now,
                UseTime   = useTime
            });
        }
    }
}