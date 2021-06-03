using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Channels;
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
using FS.Utils.Component;

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
        public IDockerOpr        DockerOpr        { get; set; }
        public IKubectlOpr       KubectlOpr       { get; set; }
        public IDockerHubService DockerHubService { get; set; }
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
                Status   = EumBuildStatus.Building,
                CreateAt = DateTime.Now
            }) > 0;

            // 没有更新成功，说明已经被抢了
            if (!isUpdate) return;

            // 项目
            var project = await ProjectService.ToInfoAsync(build.ProjectId);
            if (project == null)
            {
                await Fail(build, null);
                BuildLogService.Write(build.Id, $"项目ID={build.ProjectId}，不存在");
                return;
            }

            // Git项目
            var git = await GitService.ToInfoAsync(project.GitId);
            if (git == null)
            {
                await Fail(build, project);
                BuildLogService.Write(build.Id, $"gitID={project.GitId}，不存在");
                return;
            }

            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);

            void ActWriteLog(string output) => BuildLogService.Write(build.Id, output);
            // 定义环境变量
            var env = new BuildEnvironment
            {
                BuildNumber           = build.BuildNumber,
                ProjectName           = project.Name,
                ProjectDomain         = project.Domain,
                ProjectEntryPoint     = project.EntryPoint,
                ProjectEntryPort      = project.EntryPort,
                ProjectReleaseDirRoot = DotnetOpr.GetReleasePath(project),
                ProjectSourceDirRoot  = DotnetOpr.GetSourceDirRoot(project, git),
                DockerFilePath        = DotnetOpr.GetReleasePath(project) + "/Dockerfile",
                DockerHub             = DockerOpr.GetDockerHub(docker),
                DockerImage           = DockerOpr.GetDockerImage(docker, project, build.BuildNumber),
            };

            try
            {
                // 1、拉取Git
                build = await ToInfoAsync(build.Id);
                if (build.Status == EumBuildStatus.Finish) return; // 手动取消了

                var lstGit = await GitService.ToListAsync();
                foreach (var gitVO in lstGit)
                {
                    env.GitHub     = gitVO.Hub;
                    env.GitDirRoot = GitOpr.GetGitPath(gitVO);
                    if ((await GitOpr.PullAsync(env, build, project, gitVO, ActWriteLog)).IsError)
                    {
                        await Fail(build, project);
                        return;
                    }
                }

                // 恢复成当前git设置
                env.GitHub     = git.Hub;
                env.GitDirRoot = GitOpr.GetGitPath(git);

                await ShellTools.Run("env", "", ActWriteLog, env);

                // 2、编译
                build = await ToInfoAsync(build.Id);
                if (build.Status == EumBuildStatus.Finish) return; // 手动取消了
                if ((await DotnetOpr.Publish(env, build, project, git, ActWriteLog)).IsError)
                {
                    await Fail(build, project);
                    return;
                }

                // 3、打包
                build = await ToInfoAsync(build.Id);
                if (build.Status == EumBuildStatus.Finish) return; // 手动取消了
                if ((await DockerOpr.Build(env, build, project, ActWriteLog)).IsError)
                {
                    await Fail(build, project);
                    return;
                }

                // 4、上传镜像
                build = await ToInfoAsync(build.Id);
                if (build.Status == EumBuildStatus.Finish) return; // 手动取消了
                if ((await DockerOpr.Upload(env, build, project, ActWriteLog)).IsError)
                {
                    await Fail(build, project);
                    return;
                }

                // 修改项目的镜像版本
                project.DockerVer = build.BuildNumber.ToString();
                await ProjectService.UpdateAsync(project.Id, project.DockerVer);

                // 5、更新集群镜像版本
                build = await ToInfoAsync(build.Id);
                if (build.Status == EumBuildStatus.Finish) return; // 手动取消了
                if ((await KubectlOpr.SetImages(env, build, project, ActWriteLog)).IsError)
                {
                    await Fail(build, project);
                    return;
                }

                await Success(build, project);
            }
            catch (Exception e)
            {
                await Fail(build, project);
                BuildLogService.Write(build.Id, e.ToString());
            }
        }

        /// <summary>
        /// 创建构建任务
        /// </summary>
        public async Task<int> Add(int projectId, int clusterId)
        {
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
            BuildLogService.Write(id, "手动取消");
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
        private async Task Fail(BuildVO build, ProjectVO project)
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
                project.DicClusterVer[build.ClusterId].BuildFailId  = build.Id;
                await ProjectService.UpdateAsync(project.Id, project.DicClusterVer);
            }

            await BuilderContext.Data.Build.Where(o => o.Id == build.Id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = false,
                FinishAt  = DateTime.Now,
            });
        }

        /// <summary>
        /// 设置任务成功
        /// </summary>
        private async Task Success(BuildVO build, ProjectVO project)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, "成功执行。");

            await Success(build.ClusterId, project, build.Id);

            await BuilderContext.Data.Build.Where(o => o.Id == build.Id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = true,
                FinishAt  = DateTime.Now,
            });
        }

        /// <summary>
        /// 设置任务成功
        /// </summary>
        public Task Success(int clusterId, ProjectVO project, int buildId)
        {
            // 构建ID没有传的时候，通过版本号获取
            if (buildId == 0)
            {
                var buildNumber = project.DockerVer.ConvertType(0);
                buildId = BuilderContext.Data.Build.Where(o => o.BuildNumber == buildNumber && o.ProjectId == project.Id).GetValue(o => o.Id.GetValueOrDefault());
            }

            // 修改集群的镜像版本
            if (!project.DicClusterVer.ContainsKey(clusterId)) project.DicClusterVer[clusterId] = new();
            project.DicClusterVer[clusterId].DockerVer       = project.DockerVer;
            project.DicClusterVer[clusterId].DeploySuccessAt = DateTime.Now;
            project.DicClusterVer[clusterId].BuildSuccessId  = buildId;
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
        public Task<List<BuildVO>> ToBuildingList(int pageSize, int pageIndex) => BuilderContext.Data.Build.Select(o => new {o.Id, o.Status, o.BuildNumber, o.IsSuccess, o.ProjectId, o.CreateAt, o.FinishAt}).Desc(o => o.Id).ToListAsync(pageSize, pageIndex).MapAsync<BuildVO, BuildPO>();

        /// <summary>
        /// 查看构建信息
        /// </summary>
        public Task<BuildVO> ToInfoAsync(int id) => BuilderContext.Data.Build.Where(o => o.Id == id).ToEntityAsync().MapAsync<BuildVO, BuildPO>();
    }
}