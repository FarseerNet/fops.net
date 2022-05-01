using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.K8S.Server;
using FOPS.Application.Build.Cluster.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Application.Build.YamlTpl.Entity;
using FOPS.Infrastructure.Common;
using FS.Core.Entity;
using FS.Utils.Component;

namespace FOPS.Com.K8SServer.Deploy
{
    public class DeployService : IDeployService
    {
        public IClusterService ClusterService { get; set; }
        public IKubectlOpr     KubectlOpr     { get; set; }

        /// <summary>
        /// 发布
        /// </summary>
        public Task<RunShellResult> DeployAsync(List<ProjectDTO> lstProject, ClusterDTO clusterVO, List<YamlTplDTO> lstTpl)
        {
            if (clusterVO == null) throw new Exception("请先选择集群环境");
            var lstYaml = new List<string>();
            foreach (var projectVO in lstProject)
            {
                lstYaml.AddRange(ReplaceTemplate(projectVO: projectVO, lstTpl: lstTpl));
            }

            // 拼接已经选择的所有脚本
            var yaml = string.Join("\r\n---\r\n", lstYaml);
            return RunApplyCmd("all", yaml, clusterVO.Config);
        }

        /// <summary>
        /// 发布
        /// </summary>
        public async Task<RunShellResult> DeployAsync(ProjectDTO projectVO, ClusterDTO clusterVO, List<YamlTplDTO> lstTpl)
        {
            if (clusterVO == null) throw new Exception("请先选择集群环境");
            if (projectVO == null) throw new Exception("项目不存在");

            // 替换模板内容
            var lstYaml = ReplaceTemplate(projectVO: projectVO, lstTpl: lstTpl);

            // 拼接已经选择的所有脚本
            var yaml = string.Join("\r\n---\r\n", lstYaml);

            return await DeployAsync(projectVO.Name, yaml, clusterVO);
        }

        /// <summary>
        /// 发布
        /// </summary>
        public async Task<RunShellResult> DeployAsync(string projectName, string yaml, ClusterDTO clusterVO)
        {
            if (clusterVO == null) throw new Exception("请先选择集群环境");

            // 说明前面获取的时候，没有取Config字段
            if (string.IsNullOrWhiteSpace(clusterVO.Config))
            {
                var info = await ClusterService.ToInfoAsync(clusterVO.Id);
                clusterVO.Config = info.Config;
            }

            // kube配置文件    
            var env        = new BuildEnvironment();
            var configFile = KubectlOpr.GetConfigFile(env, clusterVO.Name);
            KubectlOpr.CreateConfigFile(env, clusterVO);

            return await RunApplyCmd("single", yaml, configFile);
        }

        /// <summary>
        /// 替换模板内容
        /// </summary>
        private List<string> ReplaceTemplate(ProjectDTO projectVO, List<YamlTplDTO> lstTpl)
        {
            var lstYaml = new List<string>();

            // 取出已选择的模板
            if (projectVO.K8STplDeployment > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplDeployment).Template);
            if (projectVO.K8STplService    > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplService).Template);
            if (projectVO.K8STplIngress    > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplIngress).Template);
            if (projectVO.K8STplConfig     > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplConfig).Template);

            // 替换模板
            for (var index = 0; index < lstYaml.Count; index++)
            {
                lstYaml[index] = TplTools.ReplaceTpl(projectVO, lstYaml[index]);
            }

            return lstYaml;
        }

        /// <summary>
        /// 生成yaml文件，并执行kubectl apply命令
        /// </summary>
        private Task<RunShellResult> RunApplyCmd(string projectName, string yamlContent, string clusterConfig)
        {
            // 将yaml文件写入临时文件
            var fileName = $"/tmp/{projectName}.yaml";
            System.IO.File.Delete(fileName);
            System.IO.File.WriteAllText(fileName, yamlContent, Encoding.UTF8);

            // 发布
            return ShellTools.Run("kubectl", $"apply -f {fileName} --kubeconfig={clusterConfig}", null, null);
        }
    }
}