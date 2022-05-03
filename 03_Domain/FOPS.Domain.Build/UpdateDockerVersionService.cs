using FOPS.Domain.Build.Project;
using FOPS.Domain.Build.Project.Repository;

namespace FOPS.Domain.Build;

/// <summary>
/// 修改构建的Docker版本
/// </summary>
public class UpdateDockerVersionService : ISingletonDependency
{
    public IProjectRepository ProjectRepository { get; set; }

    public Task Update(ProjectDO project, int clusterId, int buildId)
    {
        // 修改集群的镜像版本
        if (!project.DicClusterVer.ContainsKey(clusterId)) project.DicClusterVer[clusterId] = new();
        project.DicClusterVer[clusterId].DockerVer       = project.DockerVer;
        project.DicClusterVer[clusterId].DeploySuccessAt = DateTime.Now;
        project.DicClusterVer[clusterId].BuildSuccessId  = buildId;

        return ProjectRepository.UpdateAsync(project.Id, project.DicClusterVer);
    }
}