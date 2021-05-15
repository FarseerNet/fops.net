using FS.Core.Mapping.Attribute;
using Nest;

namespace FOPS.Com.K8sServerAA.Cluster.Dal
{
    public class ClusterPO
    {
        /// <summary>
        /// 集群名称
        /// </summary>
        [Field(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 本地kubectl配置地址
        /// </summary>
        [Field(Name = "config_name")]
        public string ConfigName { get; set; }
    }
}