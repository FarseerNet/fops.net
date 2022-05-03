using FOPS.Domain.Build.Build;
using FOPS.Domain.Build.Build.Repository;
using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FOPS.Domain.Build.DockerHub.Repository;
using FOPS.Domain.Build.Enum;
using FOPS.Domain.Build.Git.Repository;
using FOPS.Domain.Build.Project;
using FOPS.Domain.Build.Project.Repository;

namespace FOPS.Domain.Build;

/// <summary>
/// 构建服务
/// </summary>
public class BuildService : ISingletonDependency
{
    public IBuildLogDevice BuildLogDevice { get; set; }
    public IDockerDevice   DockerDevice   { get; set; }
    public IDotnetDevice   DotnetDevice   { get; set; }
    public IGitDevice      GitDevice      { get; set; }

    public IBuildRepository     BuildRepository     { get; set; }
    public IProjectRepository   ProjectRepository   { get; set; }
    public IGitRepository       GitRepository       { get; set; }
    public IDockerHubRepository DockerHubRepository { get; set; }

    public CheckDirectoryService  CheckDirectoryService  { get; set; }
    public GitService             GitService             { get; set; }
    public DotnetService          DotnetService          { get; set; }
    public DockerBuildService     DockerBuildService     { get; set; }
    public DockerPushService      DockerPushService      { get; set; }
    public ShellService           ShellService           { get; set; }
    public CopyToDistService      CopyToDistService      { get; set; }
    public KubectlSetImageService KubectlSetImageService { get; set; }

    /// <summary>
    /// 构建
    /// </summary>
    public async Task Build()
    {
        var build = await BuildRepository.GetUnBuildInfoAsync();
        if (build == null) return;

        var isUpdate = await BuildRepository.SetBuilding(build.Id);

        // 没有更新成功，说明已经被抢了
        if (isUpdate == 0) return;

        // 清除历史记录（正常不会存在，当buildId被重置时，有可能会冲突）
        BuildLogDevice.Clear(build.Id);

        var progress = BuildLogDevice.CreateProgress(build.Id);

        // 项目
        var project = await ProjectRepository.ToInfoAsync(build.ProjectId);
        if (project == null)
        {
            await Fail(build, null, progress);
            progress.Report($"项目ID={build.ProjectId}，不存在");
            return;
        }

        // Git项目
        var git = project.GitId > 0 ? await GitRepository.ToInfoAsync(project.GitId) : null;

        // 镜像仓库
        var docker = await DockerHubRepository.ToInfoAsync(project.DockerHub);

        // 定义环境变量
        var env = new BuildEnvironment
        {
            BuildId           = build.Id,
            BuildNumber       = build.BuildNumber,
            ProjectId         = project.Id,
            ProjectName       = project.Name,
            ProjectDomain     = project.Domain,
            ProjectEntryPoint = project.EntryPoint,
            ProjectEntryPort  = project.EntryPort,
            DockerHub         = DockerDevice.GetDockerHub(docker.Hub),
            DockerImage       = DockerDevice.GetDockerImage(docker.Hub, project.Name, build.BuildNumber),
            GitHub            = git?.Hub,
            // ProjectDistRoot       = DotnetDevice.GetReleasePath(project.Name),
            // ProjectSourceDirRoot  = DotnetDevice.GetSourceDirRoot(git?.Hub, project.Path),
            // ProjectDockerfilePath = DockerDevice.GetDockerfilePath(project.Name),
            ProjectGitRoot = GitDevice.GetGitPath(git?.Hub),
            GitName        = GitDevice.GetName(git?.Hub)
        };

        try
        {
            var cts = new CancellationTokenSource();
            // 打印环境变量
            progress.Report("---------------------------------------------------------");
            progress.Report($"环境变量：");
            foreach (var dirEnv in (Dictionary<string, string>)env)
            {
                progress.Report($"{dirEnv.Key}={dirEnv.Value}");
            }

            // 前置检查
            CheckDirectoryService.Check(env, progress, cts.Token);
            // 拉取主仓库及依赖仓库
            await CheckResult(GitService.CloneOrPullAndDependent(project, progress, cts.Token), build.Id);
            // 登陆镜像仓库(先登陆，如果失败了，后则面也不需要编译、打包了)
            await CheckResult(docker.LoginAsync(env, progress, cts.Token), build.Id);
            // 将需要打包的源代码，复制到dist目录
            await CheckResult(CopyToDistService.Copy(project, env, progress, cts.Token), build.Id);

            // // 根据项目的构建方式，选择对应的构建组件
            // switch (project.BuildType)
            // {
            //     case EumBuildType.DotnetPublish: // .net 编译
            //         await CheckResult(DotnetService.Build(env, progress, cts.Token), build.Id);
            //         break;
            //     case EumBuildType.Shell: // shell 编译
            //         await CheckResult(ShellService.ExecShellAsync(env, project, progress, cts.Token), build.Id);
            //         break;
            //     default: // 不编译，将源文件复制到编译目录 
            //         //await CheckResult(CopyToDistService.Copy(env, progress, cts.Token), build.Id);
            //         break;
            // }

            // docker打包

            await CheckResult(DockerBuildService.Build(project, env, progress, cts.Token), build.Id);
            // docker上传
            if (docker != null) await CheckResult(DockerPushService.Push(env, progress, cts.Token), build.Id);
            // k8s更新
            await CheckResult(KubectlSetImageService.SetImages(env, build, project, progress, cts.Token), build.Id);

            await Success(build, project, progress);
        }
        catch (Exception e)
        {
            await Fail(build, project, progress);
            if (!string.IsNullOrWhiteSpace(e.Message)) progress.Report(e.Message);
        }
        finally
        {
            BuildLogDevice.Stop(build.Id);
        }
    }

    private async Task CheckResult(Task<bool> result, int buildId)
    {
        var build = await BuildRepository.ToInfoAsync(buildId);
        if (build.Status == EumBuildStatus.Finish)
        {
            throw new Exception($"手动取消，退出构建。");
        }

        if (!await result)
        {
            throw new Exception();
        }
    }

    /// <summary>
    /// 设置任务失败
    /// </summary>
    private async Task Fail(BuildDO build, ProjectDO project, IProgress<string> progress)
    {
        progress.Report("---------------------------------------------------------");
        progress.Report("执行失败，提前退出。");

        if (project != null)
        {
            if (!project.DicClusterVer.ContainsKey(build.ClusterId))
                project.DicClusterVer[build.ClusterId] = new()
                {
                    DockerVer = "0"
                };
            project.DicClusterVer[build.ClusterId].DeployFailAt = DateTime.Now;
            project.DicClusterVer[build.ClusterId].BuildFailId  = build.Id;
            await ProjectRepository.UpdateAsync(project.Id, project.DicClusterVer);
        }

        await BuildRepository.CancelAsync(build.Id);
    }

    /// <summary>
    /// 设置任务成功
    /// </summary>
    private async Task Success(BuildDO build, ProjectDO project, IProgress<string> progress)
    {
        progress.Report("---------------------------------------------------------");
        progress.Report("成功执行。");

        await BuildRepository.SuccessAsync(build.Id);
    }
}