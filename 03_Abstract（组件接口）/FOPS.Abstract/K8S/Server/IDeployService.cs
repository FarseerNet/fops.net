using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.DI;

namespace FOPS.Abstract.K8S.Server
{
    public interface IDeployService: ISingletonDependency
    {
        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(ProjectVO projectVO, ClusterVO clusterVO, List<YamlTplVO> lstTpl);

        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(List<ProjectVO> lstProject, ClusterVO clusterVO, List<YamlTplVO> lstTpl);

        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(string projectName, string yaml, ClusterVO clusterVO);
    }
}