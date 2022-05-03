using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Git;
using FOPS.Domain.Build.Git.Repository;
using FOPS.Domain.Build.Project;

namespace FOPS.Domain.Build;

/// <summary>
/// 克隆Git仓库代码
/// </summary>
public class GitService : ISingletonDependency
{
    public IGitDevice     GitDevice     { get; set; }
    public IGitRepository GitRepository { get; set; }

    /// <summary>
    /// 根据判断是否存在Git目录，来决定返回Clone or pull
    /// </summary>
    public async Task<bool> CloneOrPull(GitDO git, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        actReceiveOutput ??= new Progress<string>();
        actReceiveOutput.Report("---------------------------------------------------------");

        // 先得到项目Git存放的物理路径
        var gitPath     = GitDevice.GetGitPath(git.Hub);
        var execSuccess = false;

        // 存在则使用pull
        if (GitDevice.ExistsGitProject(gitPath))
        {
            actReceiveOutput.Report($"开始拉取git {git.Name} 分支：{git.Branch} 仓库：{git.Hub}。");
            execSuccess = await GitDevice.Pull(gitPath, actReceiveOutput, cancellationToken);
        }
        else
        {
            actReceiveOutput.Report($"开始克隆git {git.Name} 分支：{git.Branch} 仓库：{git.Hub}。");
            await GitDevice.Clone(git.Hub, git.Branch, actReceiveOutput, cancellationToken);
        }

        if (execSuccess)
        {
            // 更新git拉取时间
            await GitRepository.UpdateAsync(git.Id, DateTime.Now);
        }
        else
            actReceiveOutput.Report($"拉取出错了。");
        return execSuccess;
    }

    /// <summary>
    /// 拉取主仓库及依赖仓库
    /// </summary>
    public async Task<bool> CloneOrPullAndDependent(ProjectDO project, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        // 读取主库及依赖库
        var lstGitIds = new List<int>
        {
            project.GitId
        };
        lstGitIds.AddRange(project.DependentGitIds);
        lstGitIds.Remove(0);

        var lstGit = await GitRepository.ToListAsync(lstGitIds);

        foreach (var git in lstGit)
        {
            if (!await CloneOrPull(git, actReceiveOutput, cancellationToken))
            {
                return false;
            }
        }
        actReceiveOutput.Report($"拉取完成。");
        return true;
        
        // BuildLogService.Write(build.Id, $"检查源文件。");
        // if (!System.IO.Directory.Exists(env.ProjectSourceDirRoot))
        // {
        //     return new RunShellResult(true, $"源文件：{env.ProjectSourceDirRoot} 不存在，请检查项目设置。");
        // }
        // return new RunShellResult(false, $"拉取完成。");
    }

    /// <summary>
    /// 消除仓库
    /// </summary>
    public async Task<bool> ClearAsync(int gitId, IProgress<string> actReceiveOutput = null)
    {
        actReceiveOutput ??= new Progress<string>();

        var git = await GitRepository.ToInfoAsync(gitId);
        return await GitDevice.ClearAsync(git.Hub, actReceiveOutput);
    }
}