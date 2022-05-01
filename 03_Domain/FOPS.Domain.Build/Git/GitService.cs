using FOPS.Domain.Build.Git.Repository;

namespace FOPS.Domain.Build.Git;

public class GitService: ISingletonDependency
{
    public IGitRepository GitRepository { get; set; }
}