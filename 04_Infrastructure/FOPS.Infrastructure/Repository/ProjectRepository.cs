using FOPS.Domain.Build.Project.Repository;
using FOPS.Infrastructure.Repository.Project;

namespace FOPS.Infrastructure.Repository;

public class ProjectRepository : IProjectRepository
{
    public ProjectAgent ProjectAgent { get; set; }
}