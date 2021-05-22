using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.K8S.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Infrastructure.Common;
using FS.Core.Entity;

namespace FOPS.Com.K8SServer.Deploy
{
    public class DeployService : IDeployService
    {
        public IBuildService BuildService { get; set; }

        /// <summary>
        /// 发布
        /// </summary>
        public Task<RunShellResult> DeployAsync(List<ProjectVO> lstProject, ClusterVO clusterVO, List<YamlTplVO> lstTpl)
        {
            if (clusterVO == null) throw new Exception("请先选择集群环境");
            var lstYaml = new List<string>();
            foreach (var projectVO in lstProject)
            {
                lstYaml.AddRange(ReplaceTemplate(projectVO: projectVO, lstTpl: lstTpl));
            }

            // 拼接已经选择的所有脚本
            var yaml = string.Join("\r\n---\r\n", lstYaml);
            return RunApplyCmd("all", yaml, clusterVO.ConfigName);
        }

        /// <summary>
        /// 发布
        /// </summary>
        public Task<RunShellResult> DeployAsync(ProjectVO projectVO, ClusterVO clusterVO, List<YamlTplVO> lstTpl)
        {
            if (clusterVO == null) throw new Exception("请先选择集群环境");
            if (projectVO == null) throw new Exception("项目不存在");

            // 替换模板内容
            var lstYaml = ReplaceTemplate(projectVO: projectVO, lstTpl: lstTpl);

            // 拼接已经选择的所有脚本
            var yaml = string.Join("\r\n---\r\n", lstYaml);
            return RunApplyCmd(projectVO.Name, yaml, clusterVO.ConfigName);
        }

        /// <summary>
        /// 发布
        /// </summary>
        public Task<RunShellResult> DeployAsync(string yaml, ClusterVO clusterVO)
        {
            if (clusterVO == null) throw new Exception("请先选择集群环境");

            return RunApplyCmd("single", yaml, clusterVO.ConfigName);
        }

        /// <summary>
        /// 替换模板内容
        /// </summary>
        private List<string> ReplaceTemplate(ProjectVO projectVO, List<YamlTplVO> lstTpl)
        {
            var lstYaml = new List<string>();

            // 取出已选择的模板
            if (projectVO.K8STplDeployment > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplDeployment).Template);
            if (projectVO.K8STplService > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplService).Template);
            if (projectVO.K8STplIngress > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplIngress).Template);
            if (projectVO.K8STplConfig > 0) lstYaml.Add(lstTpl.Find(o => o.Id == projectVO.K8STplConfig).Template);

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
        /// <param name="projectName"></param>
        /// <param name="yamlContent"></param>
        /// <param name="clusterConfig"></param>
        /// <exception cref="Exception"></exception>
        private async Task<RunShellResult> RunApplyCmd(string projectName, string yamlContent, string clusterConfig)
        {
            // 将yaml文件写入临时文件
            var fileName = $"/tmp/{projectName}.yaml";
            System.IO.File.Delete(fileName);
            System.IO.File.WriteAllText(fileName, yamlContent, Encoding.UTF8);

            // 发布
            return await ShellTools.Run("kubectl", $"apply -f {fileName} --kubeconfig={clusterConfig}", null);
        }
    }
}