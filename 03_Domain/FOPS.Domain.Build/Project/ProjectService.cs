using FOPS.Domain.Build.Project.Repository;

namespace FOPS.Domain.Build.Project;

public class ProjectService : ISingletonDependency
{
    public IProjectRepository ProjectRepository { get; set; }

    /// <summary>
    /// 修改构建的Docker版本
    /// </summary>
    /// <returns></returns>
    public Task UpdateDockerVer(ProjectDO project, int clusterId, int buildId)
    {
        // 修改集群的镜像版本
        if (!project.DicClusterVer.ContainsKey(clusterId)) project.DicClusterVer[clusterId] = new();
        project.DicClusterVer[clusterId].DockerVer       = project.DockerVer;
        project.DicClusterVer[clusterId].DeploySuccessAt = DateTime.Now;
        project.DicClusterVer[clusterId].BuildSuccessId  = buildId;

        return ProjectRepository.UpdateAsync(project.Id, project.DicClusterVer);
    }
}