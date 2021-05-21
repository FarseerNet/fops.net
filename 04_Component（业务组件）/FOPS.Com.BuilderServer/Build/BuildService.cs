using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Enum;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.BuilderServer.Build.Dal;
using FS.DI;
using FS.Extends;

namespace FOPS.Com.BuilderServer.Build
{
    /// <summary>
    /// 构建
    /// </summary>
    public class BuildService : IBuildService
    {
        public IGitOpr           GitOpr           { get; set; }
        public IDotnetOpr        DotnetOpr        { get; set; }
        public IProjectService   ProjectService   { get; set; }
        public IGitService       GitService       { get; set; }
        public IBuildLogService  BuildLogService  { get; set; }
        public IDockerHubService DockerHubService { get; set; }
        public IDockerOpr        DockerOpr        { get; set; }
        public IKubectlOpr       KubectlOpr       { get; set; }
        public IIocManager       IocManager       { get; set; }

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

            // 项目
            var project = await ProjectService.ToInfoAsync(build.ProjectId);
            if (project == null)
            {
                await Fail(build, null, 0);
                BuildLogService.Write(build.Id, $"项目ID={build.ProjectId}，不存在");
                return;
            }

            // Git项目
            var git = await GitService.ToInfoAsync(project.GitId);
            if (git == null)
            {
                await Fail(build, project, 0);
                BuildLogService.Write(build.Id, $"gitID={project.GitId}，不存在");
                return;
            }

            var startNew = Stopwatch.StartNew();

            void ActWriteLog(string output) => BuildLogService.Write(build.Id, output);

            try
            {
                // 1、拉取Git
                var lstGit = await GitService.ToListAsync();
                foreach (var gitVO in lstGit)
                {
                    if ((await GitOpr.PullAsync(build, project, git, ActWriteLog)).IsError)
                    {
                        await Fail(build, project, startNew.ElapsedMilliseconds);
                        return;
                    }
                }

                // 2、编译
                if ((await DotnetOpr.Publish(build, project, git, ActWriteLog)).IsError)
                {
                    await Fail(build, project, startNew.ElapsedMilliseconds);
                    return;
                }

                // 3、打包
                if ((await DockerOpr.Build(build, project, ActWriteLog)).IsError)
                {
                    await Fail(build, project, startNew.ElapsedMilliseconds);
                    return;
                }

                // 4、上传镜像
                if ((await DockerOpr.Upload(build, project, ActWriteLog)).IsError)
                {
                    await Fail(build, project, startNew.ElapsedMilliseconds);
                    return;
                }

                // 修改项目的镜像版本
                project.DockerVer = build.BuildNumber.ToString();
                await ProjectService.UpdateAsync(project.Id, project.DockerVer);

                // 5、更新集群镜像版本
                if ((await KubectlOpr.SetImages(build, project, ActWriteLog)).IsError)
                {
                    await Fail(build, project, startNew.ElapsedMilliseconds);
                    return;
                }

                await Success(build, project, startNew.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                await Fail(build, project, startNew.ElapsedMilliseconds);
                BuildLogService.Write(build.Id, e.ToString());
            }
        }

        /// <summary>
        /// 创建构建任务
        /// </summary>
        public async Task<int> Add(int projectId, int clusterId)
        {
            //var isHave = await BuilderContext.Data.Build.Where(o => o.ProjectId == projectId && (o.Status == EumBuildStatus.None || o.Status == EumBuildStatus.Building)).IsHavingAsync();
            //if (isHave) throw new Exception("当前队列存在未完成的构建");

            // 获取数据库中最后一个编译版本号
            var buildNumber = await BuilderContext.Data.Build.Where(o => o.ProjectId == projectId).Desc(o => o.Id).GetValueAsync(o => o.BuildNumber.GetValueOrDefault());
            var po = new BuildPO
            {
                ProjectId   = projectId,
                ClusterId   = clusterId,
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
        private async Task Fail(BuildVO build, ProjectVO project, long useTime)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, "执行失败，提前退出。");

            if (project != null)
            {
                if (!project.DicClusterVer.ContainsKey(build.ClusterId))
                    project.DicClusterVer[build.ClusterId] = new()
                    {
                        DockerVer = "0"
                    };
                project.DicClusterVer[build.ClusterId].DeployFailAt = DateTime.Now;
                await ProjectService.UpdateAsync(project.Id, project.DicClusterVer);
            }

            await BuilderContext.Data.Build.Where(o => o.Id == build.Id).UpdateAsync(new BuildPO
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
        private async Task Success(BuildVO build, ProjectVO project, long useTime)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, "成功执行。");

            await Success(build.ClusterId, project);

            await BuilderContext.Data.Build.Where(o => o.Id == build.Id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = true,
                FinishAt  = DateTime.Now,
                UseTime   = useTime,
            });
        }

        /// <summary>
        /// 设置任务成功
        /// </summary>
        public Task Success(int clusterId, ProjectVO project)
        {
            // 修改集群的镜像版本
            if (!project.DicClusterVer.ContainsKey(clusterId)) project.DicClusterVer[clusterId] = new();
            project.DicClusterVer[clusterId].DockerVer       = project.DockerVer;
            project.DicClusterVer[clusterId].DeploySuccessAt = DateTime.Now;
            return ProjectService.UpdateAsync(project.Id, project.DicClusterVer);
        }

        /// <summary>
        /// 当前构建的队列数量
        /// </summary>
        public Task<int> CountAsync() => BuilderContext.Data.Build.Where(o => o.Status != EumBuildStatus.Finish).CountAsync();

        /// <summary>
        /// 获取构建队列前30
        /// </summary>
        /// <returns></returns>
        public Task<List<BuildVO>> ToBuildingList() => BuilderContext.Data.Build.Desc(o=>o.Id).ToListAsync(20).MapAsync<BuildVO,BuildPO>();
    }
}