using FOPS.Abstract.MetaInfo.Entity;

namespace FOPS.Infrastructure.Common
{
    public class TplTools
    {
        /// <summary>
        /// 替换模板
        /// </summary>
        public static string ReplaceTpl(ProjectVO project, string tpl)
        {
            // 替换项目名称
            tpl = tpl.Replace("${project_name}", project.Name)
                .Replace("${domain}",      project.Domain)
                .Replace("${entry_point}", project.EntryPoint)
                .Replace("${entry_port}",  project.EntryPort.ToString());

            // 替换模板变量
            foreach (var kv in project.K8STplVariable.Split(','))
            {
                var kvGroup = kv.Split('=');
                if (kvGroup.Length != 2) continue;
                tpl = tpl.Replace($"${{{kvGroup[0]}}}", kvGroup[1]);
            }

            return tpl;
        }
    }
}