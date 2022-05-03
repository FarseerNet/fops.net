using FOPS.Domain.Build.Cluster;
using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Project;
using FOPS.Domain.Build.YamlTpl;

namespace FOPS.Domain.Build
{
    public class KubectlSetYamlService : ISingletonDependency
    {
        public IKubectlDevice KubectlDevice { get; set; }

        /// <summary>
        /// 发布
        /// </summary>
        public Task<bool> DeployAsync(List<ProjectDO> lstProject, ClusterDO cluster, List<YamlTplDO> lstTpl, IProgress<string> progress, CancellationToken cancellationToken)
        {
            if (cluster == null) throw new Exception("请先选择集群环境");
            var lstYaml = new List<string>();
            foreach (var projectVO in lstProject)
            {
                lstYaml.AddRange(ReplaceTemplate(projectDTO: projectVO, lstTpl: lstTpl));
            }

            KubectlDevice.CreateConfigFile(cluster.Name, cluster.Config);
            
            // 拼接已经选择的所有脚本
            var yaml = string.Join("\r\n---\r\n", lstYaml);
            return KubectlDevice.SetYaml(cluster.Name, "all", yaml, progress, cancellationToken);
        }

        /// <summary>
        /// 发布
        /// </summary>
        public Task<bool> DeployAsync(ProjectDO project, ClusterDO cluster, List<YamlTplDO> lstTpl, IProgress<string> progress, CancellationToken cancellationToken)
        {
            if (cluster == null) throw new Exception("请先选择集群环境");
            if (project == null) throw new Exception("项目不存在");

            // 替换模板内容
            var lstYaml = ReplaceTemplate(projectDTO: project, lstTpl: lstTpl);

            KubectlDevice.CreateConfigFile(cluster.Name, cluster.Config);
            
            // 拼接已经选择的所有脚本
            var yaml = string.Join("\r\n---\r\n", lstYaml);
            return KubectlDevice.SetYaml(cluster.Name, "single", yaml, progress, cancellationToken);
        }
        
        /// <summary>
        /// 发布
        /// </summary>
        public Task<bool> DeployAsync(ClusterDO cluster, string yaml, IProgress<string> progress, CancellationToken cancellationToken)
        {
            KubectlDevice.CreateConfigFile(cluster.Name, cluster.Config);
            return KubectlDevice.SetYaml(cluster.Name, "single", yaml, progress, cancellationToken);
        }

        /// <summary>
        /// 替换模板内容
        /// </summary>
        private List<string> ReplaceTemplate(ProjectDO projectDTO, List<YamlTplDO> lstTpl)
        {
            var lstYaml = new List<string>();

            // 取出已选择的模板
            if (projectDTO.K8STplDeployment > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectDTO.K8STplDeployment).Template);
            if (projectDTO.K8STplService    > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectDTO.K8STplService).Template);
            if (projectDTO.K8STplIngress    > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectDTO.K8STplIngress).Template);
            if (projectDTO.K8STplConfig     > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectDTO.K8STplConfig).Template);

            // 替换模板
            for (var index = 0; index < lstYaml.Count; index++)
            {
                lstYaml[index] = projectDTO.ReplaceTpl(lstYaml[index]);
            }
            return lstYaml;
        }
    }
}