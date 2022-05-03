using FOPS.Domain.Build.DeployK8S.Entity;

namespace FOPS.Domain.Build;

/// <summary>
/// 检查目录
/// </summary>
public class CheckDirectoryService : ISingletonDependency
{
    public void Check(BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken)
    {
        progress.Report("---------------------------------------------------------");
        progress.Report($"前置检查。");

        // 自动创建目录
        progress.Report($"自动创建目录。");
        if (!Directory.Exists(BuildEnvironment.FopsDirRoot)) Directory.CreateDirectory(BuildEnvironment.FopsDirRoot);
        if (!Directory.Exists(BuildEnvironment.NpmModulesDirRoot)) Directory.CreateDirectory(BuildEnvironment.NpmModulesDirRoot);
        if (!Directory.Exists(BuildEnvironment.ReleaseDirRoot)) Directory.CreateDirectory(BuildEnvironment.ReleaseDirRoot);
        if (!Directory.Exists(BuildEnvironment.KubePath)) Directory.CreateDirectory(BuildEnvironment.KubePath);
        if (!Directory.Exists(BuildEnvironment.ShellScriptPath)) Directory.CreateDirectory(BuildEnvironment.ShellScriptPath);
        if (!Directory.Exists(BuildEnvironment.GitDirRoot)) Directory.CreateDirectory(BuildEnvironment.GitDirRoot);

        // 先删除之前编译的目标文件
        progress.Report($"先删除之前编译的目标文件。");
        if (Directory.Exists(env.ProjectReleaseDirRoot)) Directory.Delete(env.ProjectReleaseDirRoot, true);
        Directory.CreateDirectory(env.ProjectReleaseDirRoot);

        progress.Report($"前置检查通过。");
    }
}