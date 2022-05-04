using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
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
        //var sourceDockerfilePath = $"{env.ProjectSourceDirRoot}/Dockerfile";

        // // 如果文件存在，则使用项目中的定义的文件
        // if (DockerDevice.ExistsDockerfile(sourceDockerfilePath))
        // {
        //     progress.Report($"项目中包含Dockerfile，将按存在的Dockerfile进行打包");
        //     File.Copy(sourceDockerfilePath, env.ProjectDockerfilePath, true);
        //     return true;
        // }

        //progress.Report($"未发现Dockerfile，将按模板创建");

        // 根据项目中的Dockerfile模板ID，取出模板
        var dockerfileTpl = await DockerfileTplRepository.ToInfoAsync(project.DockerfileTpl);
        if (dockerfileTpl == null)
        {
            progress.Report($"DockerfileTpl={project.DockerfileTpl}，不存在");
            return false;
        }

        // 替换模板
        var tpl = project.ReplaceTpl(dockerfileTpl.Template);
        tpl = tpl.Replace("${git_name}", env.GitName);

        // 如果.net 应用，则自动实现csproj的递归复制并运行dotnet restore
        var lstCopyCmd = new List<string>();
        var csproj     = Directory.GetFiles(BuildEnvironment.DistRoot, "*.csproj", SearchOption.AllDirectories);
        foreach (var file in csproj)
        {
            var filePath = file.Substring(BuildEnvironment.DistRoot.Length);
            var fileDir  = filePath.Substring(0, filePath.LastIndexOf('/') + 1);
            lstCopyCmd.Add($"COPY [\"{filePath}\",\"{fileDir}\"]");
        }
        lstCopyCmd.Add($"RUN dotnet restore {env.GitName}/{project.Path}/ -s https://nuget.cdn.azure.cn/v3/index.json");

        tpl = tpl.Replace("${dotnet_restore}", string.Join("\r\n", lstCopyCmd));
        await DockerDevice.CreateDockerfileAsync(project.Name, tpl, cancellationToken);
        return true;
    }
}