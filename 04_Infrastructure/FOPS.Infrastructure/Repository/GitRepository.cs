using FOPS.Domain.Build.Git.Repository;
using FOPS.Infrastructure.Repository.Git;

namespace FOPS.Infrastructure.Repository;

public class GitRepository : IGitRepository
{
    public GitAgent GitAgent { get; set; }
}