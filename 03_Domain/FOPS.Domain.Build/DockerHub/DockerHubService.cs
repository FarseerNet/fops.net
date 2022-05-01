using FOPS.Domain.Build.DockerHub.Repository;

namespace FOPS.Domain.Build.DockerHub;

public class DockerHubService: ISingletonDependency
{
    public IDockerHubRepository DockerHubRepository { get; set; }
}