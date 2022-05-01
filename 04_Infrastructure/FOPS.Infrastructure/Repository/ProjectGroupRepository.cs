using FOPS.Domain.Build.ProjectGroup.Repository;
using FOPS.Infrastructure.Repository.ProjectGroup;

namespace FOPS.Infrastructure.Repository;

public class ProjectGroupRepository : IProjectGroupRepository
{
    public ProjectGroupAgent ProjectGroupAgent { get; set; }
}