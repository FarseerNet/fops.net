using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FS.Utils.Component;

namespace FOPS.Infrastructure.Device;

public class DotnetDevice : IDotnetDevice
{
    public IGitDevice GitDevice { get; set; }

    /// <summary>
    /// 获取项目源地址
    /// </summary>
    public string GetSourceDirRoot(string github, string projectPath)
    {
        return GitDevice.GetGitPath(github) + (projectPath.StartsWith("/") ? projectPath.Substring(1) : projectPath);
    }

    /// <summary>
    /// 获取编译保存的目录地址
    /// </summary>
    public string GetReleasePath(string projectName) => BuildEnvironment.ReleaseDirRoot + projectName;

    /// <summary>
    /// 检查项目文件是否存在
    /// </summary>
    public bool CheckExistsSource(BuildEnvironment env, IProgress<string> receiveOutput)
    {
        if (!Directory.Exists(env.ProjectSourceDirRoot))
        {
            receiveOutput.Report($"路径：{env.ProjectSourceDirRoot}不存在，无法编译");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 编译.net core
    /// </summary>
    public async Task<bool> Publish(BuildEnvironment env, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        var exitCode = await ShellTools.Run("dotnet", $"restore", actReceiveOutput, env, env.ProjectSourceDirRoot, cancellationToken);
        if (exitCode != 0) return false;

        exitCode = await ShellTools.Run("dotnet", $"publish -c Release -o {env.ProjectReleaseDirRoot}", actReceiveOutput, env, env.ProjectSourceDirRoot, cancellationToken);
        return exitCode == 0;
    }

    /// <summary>
    /// 编译.net core
    /// </summary>
    public async Task<bool> Publish(string savePath, string source, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        var exitCode = await ShellTools.Run("dotnet", $"restore", actReceiveOutput, null, source);
        if (exitCode != 0) return false;

        exitCode = await ShellTools.Run("dotnet", $"publish -c Release -o {savePath}", actReceiveOutput, null, source, cancellationToken);
        return exitCode == 0;
    }
}