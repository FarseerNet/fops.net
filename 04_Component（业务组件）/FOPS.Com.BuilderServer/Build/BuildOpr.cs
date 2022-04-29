using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Enum;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Enum;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Com.BuilderServer.Build.Dal;
using FOPS.Com.BuilderServer.Docker;
using FOPS.Com.BuilderServer.Dotnet;
using FOPS.Com.BuilderServer.Git;
using FOPS.Com.BuilderServer.Kubectl;
using FOPS.Com.BuilderServer.Shell;
using FOPS.Com.BuilderServer.UnBuild;
using FS;
using FS.DI;
using FS.Extends;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Build
{
    public class BuildOpr : IBuildOpr
    {
        public IGitOpr           GitOpr           { get; set; }
        public IDotnetOpr        DotnetOpr        { get; set; }
        public IProjectService   ProjectService   { get; set; }
        public IGitService       GitService       { get; set; }
        public IBuildLogService  BuildLogService  { get; set; }
        public IDockerOpr        DockerOpr        { get; set; }
        public IDockerHubService DockerHubService { get; set; }
        public IBuildService     BuildService     { get; set; }
        public IIocManager       IocManager       { get; set; }

        /// <summary>
        /// 构建
        /// </summary>
        public async Task Build()
        {
            // 取出未开始的任务
            var po = await BuilderContext.Data.Build.Where(o => o.Status == EumBuildStatus.None && o.BuildServerId == FarseerApplication.AppId).Asc(o => o.Id).ToEntityAsync();
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

            // 清除历史记录（正常不会存在，当buildId被重置时，有可能会冲突）
            BuildLogService.Clear(build.Id);

            // 项目
            var project = await ProjectService.ToInfoAsync(build.ProjectId);
            if (project == null)
            {
                await Fail(build, null);
                BuildLogService.Write(build.Id, $"项目ID={build.ProjectId}，不存在");
                return;
            }

            // Git项目
            var git = project.GitId > 0 ? await GitService.ToInfoAsync(project.GitId) : null;

            // 镜像仓库
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);

            // 执行命令时需要记录日志到文件
            void ActWriteLog(string output) => BuildLogService.Write(build.Id, output);
            // 定义环境变量
            var env = new BuildEnvironment
            {
                BuildNumber       = build.BuildNumber,
                ProjectName       = project.Name,
                ProjectDomain     = project.Domain,
                ProjectEntryPoint = project.EntryPoint,
                ProjectEntryPort  = project.EntryPort,
                DockerHub         = DockerOpr.GetDockerHub(docker),
                DockerImage       = DockerOpr.GetDockerImage(docker, project, build.BuildNumber),
                GitHub            = git?.Hub
            };

            env.ProjectReleaseDirRoot = DotnetOpr.GetReleasePath(env, project);
            env.DockerFilePath        = env.ProjectReleaseDirRoot + "/Dockerfile";
            env.ProjectGitDirRoot     = GitOpr.GetGitPath(env, git);
            env.ProjectSourceDirRoot  = DotnetOpr.GetSourceDirRoot(env, project, git);

            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                // 打印环境变量
                BuildLogService.Write(build.Id, "---------------------------------------------------------");
                BuildLogService.Write(build.Id, $"打印环境变量。");
                foreach (var dirEnv in (Dictionary<string, string>)env)
                {
                    BuildLogService.Write(build.Id, $"{dirEnv.Key}={dirEnv.Value}");
                }

                List<IBuildStep> lstStep = new();
                lstStep.Add(IocManager.Resolve<CheckStep>());                           // 前置检查
                lstStep.Add(IocManager.Resolve<GitPullAllStep>());                      // 拉取全部git
                if (docker != null) lstStep.Add(IocManager.Resolve<DockerLoginStep>()); // 登陆镜像仓库(先登陆，如果失败了，后则面也不需要编译、打包了)

                // 根据项目的构建方式，选择对应的构建组件
                if (project.BuildType == EumBuildType.DotnetPublish) // .net 编译
                {
                    lstStep.Add(IocManager.Resolve<DotnetBuildStep>());
                }
                else if (project.BuildType == EumBuildType.Shell) // shell 编译
                {
                    lstStep.Add(IocManager.Resolve<ShellStep>());
                }
                else lstStep.Add(IocManager.Resolve<CopyToDistStep>()); // 不编译，将源文件复制到编译目录 

                lstStep.Add(IocManager.Resolve<DockerBuildStep>()); // docker打包

                if (docker != null) lstStep.Add(IocManager.Resolve<DockerUploadStep>()); // docker上传
                lstStep.Add(IocManager.Resolve<KubectlUpdateImageStep>());               // k8s更新

                // 按步骤顺序构建
                foreach (var buildStep in lstStep)
                {
                    build = await BuildService.ToInfoAsync(build.Id);
                    if (build.Status == EumBuildStatus.Finish)
                    {
                        cts.Cancel();
                        return; // 手动取消了
                    }

                    var runShellResult = await buildStep.Build(env, build, project, git, ActWriteLog, cts.Token);

                    if (runShellResult.Output.Count > 0) BuildLogService.Write(build.Id, runShellResult.OutputLine);
                    if (runShellResult.IsError)
                    {
                        await Fail(build, project);
                        return;
                    }
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

            await BuildService.Success(build.ClusterId, project, build.Id);

            await BuilderContext.Data.Build.Where(o => o.Id == build.Id).UpdateAsync(new BuildPO
            {
                Status    = EumBuildStatus.Finish,
                IsSuccess = true,
                FinishAt  = DateTime.Now,
            });
        }
    }
}