using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.DeployK8S.Entity;
using FOPS.Domain.Build.DockerfileTpl.Repository;
using FOPS.Domain.Build.Project;

namespace FOPS.Domain.Build;

/// <summary>
/// 容器打包
/// </summary>
public class DockerBuildService : ISingletonDependency
{
    public IDockerDevice            DockerDevice            { get; set; }
    public IDockerfileTplRepository DockerfileTplRepository { get; set; }

    /// <summary>
    /// 容器构建
    /// </summary>
    public async Task<bool> Build(ProjectDO project, BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken = default)
    {
        progress.Report("---------------------------------------------------------");
        progress.Report($"开始镜像打包。");

        // 生成Dockerfile文件
        if (!await CreateDockerfile(project, env, progress, cancellationToken)) return false;

        // 打包
        return await DockerDevice.Build(env, progress, cancellationToken);
    }

    /// <summary>
    /// 生成Dockerfile文件
    /// </summary>
    public async Task<bool> CreateDockerfile(ProjectDO project, BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken = default)
    {
        // 如果文件存在，则使用项目中的定义的文件
        if (DockerDevice.ExistsDockerfile(env.DockerFilePath))
        {
            progress.Report($"项目中包含Dockerfile，将按存在的Dockerfile进行打包");
            return true;
        }

        progress.Report($"未发现Dockerfile，将按模板创建");

        // 根据项目中的Dockerfile模板ID，取出模板
        var dockerfileTpl = await DockerfileTplRepository.ToInfoAsync(project.DockerfileTpl);
        if (dockerfileTpl == null)
        {
            progress.Report($"DockerfileTpl={project.DockerfileTpl}，不存在");
            return false;
        }

        // 替换模板
        var tpl = project.ReplaceTpl(dockerfileTpl.Template);
        await DockerDevice.CreateDockerfileAsync(env.DockerFilePath, tpl, cancellationToken);
        return true;
    }
}