using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;

namespace FOPS.Domain.Build;

public class DotnetService : ISingletonDependency
{
    public IDotnetDevice DotnetDevice { get; set; }
    
    public async Task<bool> Build(BuildEnvironment env, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        actReceiveOutput.Report("---------------------------------------------------------");
        actReceiveOutput.Report($"开始编译。");

        if (!Directory.Exists(env.ProjectGitRoot))
        {
            actReceiveOutput.Report($"路径：{env.ProjectGitRoot}不存在，无法编译");
            return false;
        }

        // 编译
        var execSuccess = await DotnetDevice.Publish(env, actReceiveOutput, cancellationToken);
        actReceiveOutput.Report(execSuccess ? $"编译完成。" : $"编译出错了。");

        return execSuccess;
    }
}