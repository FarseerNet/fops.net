using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.DeployK8S.Entity;
using FOPS.Domain.Build.Project;

namespace FOPS.Domain.Build;

/// <summary>
/// Shell工具
/// </summary>
public class ShellService : ISingletonDependency
{
    public IShellDevice ShellDevice { get; set; }

    public Task<bool> ExecShellAsync(BuildEnvironment env, ProjectDO project, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        actReceiveOutput.Report("---------------------------------------------------------");
        actReceiveOutput.Report($"开始执行Shell脚本。");
        actReceiveOutput.Report($"请注意：脚本执行完后，请自行将打包的文件复制到：{env.ProjectReleaseDirRoot}。");

        return ShellDevice.ExecShellAsync(env, project.ShellScript, actReceiveOutput, cancellationToken);
    }
}