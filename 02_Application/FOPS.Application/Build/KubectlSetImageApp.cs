using FOPS.Application.Build.Build.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Domain.Build;
using FOPS.Domain.Build.Deploy.Entity;

namespace FOPS.Application.Build;

public class KubectlSetImageApp : ISingletonDependency
{
    public KubectlSetImageService KubectlSetImageService { get; set; }

    public Task<bool> SetImages(BuildEnvironment env, BuildDTO build, ProjectDTO project, IProgress<string> progress, CancellationToken cancellationToken)
    {
        return KubectlSetImageService.SetImages(env, build, project, progress, cancellationToken);
    }

    public Task<bool> SyncImages(int clusterId, ProjectDTO project, IProgress<string> progress)
    {
        return KubectlSetImageService.SyncImages(clusterId, project, progress);
    }
}