using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FS.Utils.Component;

namespace FOPS.Infrastructure.Device;

/// <summary>
/// Shell工具
/// </summary>
public class ShellDevice : IShellDevice
{
    /// <summary>
    /// 执行Shell脚本
    /// </summary>
    /// <returns></returns>
    public async Task<bool> ExecShellAsync(BuildEnvironment env, string shellScript, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        // 每次执行时，需要生成shell脚本
        var path = BuildEnvironment.ShellRoot + $"fops_{env.BuildId}.sh";
        await File.AppendAllTextAsync(path, shellScript, cancellationToken);

        // 执行脚本
        var exitCode = await ShellTools.Run("/bin/sh", $"-xe {path}", actReceiveOutput, env, BuildEnvironment.DistRoot, cancellationToken);

        actReceiveOutput.Report(exitCode == 0 ? "执行脚本完成。" : "执行脚本出错了。");
        return exitCode == 0;
    }
}