using FOPS.Application.Build.Cluster.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Application.Build.YamlTpl.Entity;
using FOPS.Domain.Build;
using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Project;
using FOPS.Domain.Build.YamlTpl;

namespace FOPS.Application.Build;

public class KubectlSetYamlApp : ISingletonDependency
{
    public KubectlSetYamlService KubectlSetYamlService { get; set; }
    public IKubectlDevice        KubectlDevice         { get; set; }

    /// <summary>
    /// 发布
    /// </summary>
    public Task<bool> DeployAsync(List<ProjectDTO> lstProject, ClusterDTO clusterDTO, List<YamlTplDTO> lstTpl, IProgress<string> progress)
    {
        var projectList = lstProject.Select(o => (ProjectDO)o).ToList();
        var yamlTplList = lstTpl.Select(o => (YamlTplDO)o).ToList();

        return KubectlSetYamlService.DeployAsync(projectList, clusterDTO, yamlTplList, progress, default);
    }

    /// <summary>
    /// 发布
    /// </summary>
    public Task<bool> DeployAsync(ProjectDTO projectDTO, ClusterDTO clusterDTO, List<YamlTplDTO> lstTpl, IProgress<string> progress)
    {
        var yamlTplList = lstTpl.Select(o => (YamlTplDO)o).ToList();
        return KubectlSetYamlService.DeployAsync(projectDTO, clusterDTO, yamlTplList, progress, default);
    }

    /// <summary>
    /// 发布
    /// </summary>
    public Task<bool> DeployAsync(ClusterDTO clusterDTO, string yaml, IProgress<string> progress)
    {
        return KubectlSetYamlService.DeployAsync(clusterDTO, yaml, progress, default);
    }
}