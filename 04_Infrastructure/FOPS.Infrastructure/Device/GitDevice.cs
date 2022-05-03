using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FS.Utils.Component;

namespace FOPS.Infrastructure.Device;

public class GitDevice : IGitDevice
{
    /// <summary>
    /// 获取项目GIT源代码存的位置/var/lib/fops/git/{gitName}/
    /// </summary>
    public string GetGitPath(string gitHub)
    {
        if (string.IsNullOrWhiteSpace(gitHub)) return string.Empty;
        var gitName                           = gitHub.Substring(gitHub.LastIndexOf('/') + 1);
        if (gitName.EndsWith(".git")) gitName = gitName.Substring(0, gitName.Length - 4);
        return BuildEnvironment.GitDirRoot + gitName + "/";
    }

    /// <summary>
    /// 记住密码
    /// </summary>
    public Task RememberPassword(BuildEnvironment env, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        // 让git记住密码
        return ShellTools.Run("git", "config --global credential.helper store", actReceiveOutput, env, null, cancellationToken);
    }

    /// <summary>
    /// 项目GIT是否存在
    /// </summary>
    public bool ExistsGitProject(string gitPath)
    {
        // 如果Git存放的目录不存在，则创建
        if (!Directory.Exists(BuildEnvironment.GitDirRoot)) Directory.CreateDirectory(BuildEnvironment.GitDirRoot);

        return Directory.Exists(gitPath);
    }

    /// <summary>
    /// 克隆
    /// </summary>
    public async Task<bool> Clone(string github, string branch, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        var gitPath  = GetGitPath(github);
        var exitCode = await ShellTools.Run("git", $"clone -b {branch} {github} {gitPath}", actReceiveOutput, null, null, cancellationToken);
        if (exitCode != 0)
        {
            actReceiveOutput.Report("Git克隆失败");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 拉取最新代码
    /// </summary>
    public async Task<bool> Pull(string savePath, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        var exitCode = await ShellTools.Run("git", $"-C {savePath} pull --rebase", actReceiveOutput, null, null, cancellationToken);
        if (exitCode != 0)
        {
            actReceiveOutput.Report("Git拉取失败");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 消除仓库
    /// </summary>
    public async Task<bool> ClearAsync(string github, IProgress<string> actReceiveOutput)
    {
        // 获取Git存放的路径
        var gitPath  = GetGitPath(github);
        var exitCode = await ShellTools.Run("rm", $"-rf {gitPath}", actReceiveOutput, null);
        if (exitCode != 0)
        {
            actReceiveOutput.Report("Git清除失败");
            return false;
        }
        return true;
    }
}