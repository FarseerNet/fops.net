using FOPS.Domain.Build.Build.Repository;
using FOPS.Infrastructure.Repository.Build;

namespace FOPS.Infrastructure.Repository;

public class BuildRepository : IBuildRepository
{
    public BuildAgent BuildAgent { get; set; }
}