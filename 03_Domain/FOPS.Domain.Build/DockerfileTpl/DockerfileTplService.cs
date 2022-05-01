using FOPS.Domain.Build.DockerfileTpl.Repository;

namespace FOPS.Domain.Build.DockerfileTpl;

public class DockerfileTplService : ISingletonDependency
{
    public IDockerfileTplRepository DockerfileTplRepository { get; set; }
    
}