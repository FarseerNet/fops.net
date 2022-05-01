using FOPS.Domain.Build.Cluster.Repository;

namespace FOPS.Domain.Build.Cluster;

public class ClusterService : ISingletonDependency
{
    public IClusterRepository ClusterRepository { get; set; }
    
}