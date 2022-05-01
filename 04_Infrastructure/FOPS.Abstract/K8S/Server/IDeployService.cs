using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Application.Build.Cluster.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Application.Build.YamlTpl.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.K8S.Server
{
    public interface IDeployService : ISingletonDependency
    {
        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(ProjectDTO projectVO, ClusterDTO clusterVO, List<YamlTplDTO> lstTpl);

        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(List<ProjectDTO> lstProject, ClusterDTO clusterVO, List<YamlTplDTO> lstTpl);

        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(string projectName, string yaml, ClusterDTO clusterVO);
    }
}