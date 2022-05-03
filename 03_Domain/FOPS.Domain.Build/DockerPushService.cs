using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.DeployK8S.Entity;
using FOPS.Domain.Build.Project.Repository;

namespace FOPS.Domain.Build;

/// <summary>
/// 上传镜像
/// </summary>
public class DockerPushService : ISingletonDependency
{
    public IDockerDevice      DockerDevice      { get; set; }
    public IProjectRepository ProjectRepository { get; set; }

    public async Task<bool> Push(BuildEnvironment env, IProgress<string> actReceiveOutput, CancellationToken cancellationToken)
    {
        actReceiveOutput.Report("---------------------------------------------------------");
        actReceiveOutput.Report($"开始上传镜像。");

        // 上传镜像
        var pushResult = await DockerDevice.Push(env, actReceiveOutput, cancellationToken);
        
        // 上传成功后，需要更新项目中的镜像版本属性
        if (pushResult)
        {
            await ProjectRepository.UpdateAsync(env.ProjectId, env.BuildNumber.ToString());
        }
        return pushResult;
    }
}