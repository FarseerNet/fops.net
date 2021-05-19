using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.K8S.Enum;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.K8SServer.YamlTpl.Dal
{
    [Map(typeof(YamlTplVO))]
    public class YamlTplPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true,IsDbGenerated = true)]
        public int? Id { get; set; }
        
        /// <summary>
        /// k8s类型
        /// </summary>
        [Field(Name = "k8s_kind_type")]
        public EumK8SKind? K8SKindType { get; set; }
        
        /// <summary>
        /// 模板名称
        /// </summary>
        [Field(Name = "name")]
        public string Name { get; set; }
        
        /// <summary>
        /// 模板内容
        /// </summary>
        [Field(Name = "template")]
        public string Template { get; set; }
    }
}