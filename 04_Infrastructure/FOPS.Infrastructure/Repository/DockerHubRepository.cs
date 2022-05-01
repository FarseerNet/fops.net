using FOPS.Domain.Build.DockerHub.Repository;
using FOPS.Infrastructure.Repository.DockerHub;

namespace FOPS.Infrastructure.Repository;

public class DockerHubRepository : IDockerHubRepository
{
    public DockerHubAgent DockerHubAgent { get; set; }
}