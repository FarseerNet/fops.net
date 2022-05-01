using FOPS.Domain.Build.DockerfileTpl.Repository;
using FOPS.Infrastructure.Repository.DockerfileTpl;

namespace FOPS.Infrastructure.Repository;

public class DockerfileTplRepository : IDockerfileTplRepository
{
    public DockerfileTplAgent DockerfileTplAgent { get; set; }
}