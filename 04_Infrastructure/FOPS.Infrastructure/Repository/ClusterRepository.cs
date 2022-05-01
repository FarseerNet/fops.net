using FOPS.Domain.Build.Cluster.Repository;
using FOPS.Infrastructure.Repository.Cluster;

namespace FOPS.Infrastructure.Repository;

public class ClusterRepository : IClusterRepository
{
    public ClusterAgent ClusterAgent { get; set; }
}