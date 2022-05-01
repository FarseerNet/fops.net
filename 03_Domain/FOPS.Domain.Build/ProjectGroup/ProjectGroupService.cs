using FOPS.Domain.Build.ProjectGroup.Repository;

namespace FOPS.Domain.Build.ProjectGroup;

public class ProjectGroupService: ISingletonDependency
{
    public IProjectGroupRepository ProjectGroupRepository { get; set; }
}