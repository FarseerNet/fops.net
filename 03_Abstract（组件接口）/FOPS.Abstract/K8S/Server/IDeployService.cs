using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.DI;

namespace FOPS.Abstract.K8S.Server
{
    public interface IDeployService: ITransientDependency
    {
        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(ProjectVO projectVO, ClusterVO clusterVO, List<YamlTplVO> lstTpl);

        /// <summary>
        /// 发布
        /// </summary>
        Task<RunShellResult> DeployAsync(List<ProjectVO> lstProject, ClusterVO clusterVO, List<YamlTplVO> lstTpl);
    }
}