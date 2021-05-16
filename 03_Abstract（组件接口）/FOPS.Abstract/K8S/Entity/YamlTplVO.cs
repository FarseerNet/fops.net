using FOPS.Abstract.K8S.Enum;

namespace FOPS.Abstract.K8S.Entity
{
    public class YamlTplVO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// k8s类型
        /// </summary>
        public EumK8SKind K8SKindType { get; set; }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 模板内容
        /// </summary>
        public string Template { get; set; }
    }
}