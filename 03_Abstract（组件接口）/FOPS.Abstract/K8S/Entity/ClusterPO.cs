using FOPS.Abstract.MetaInfo.Enum;

namespace FOPS.Abstract.K8S.Entity
{
    public class ClusterVO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 集群名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 本地kubectl配置地址
        /// </summary>
        public string Config { get; set; }
        
        /// <summary>
        /// 排序（越小越前）
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 集群环境类型
        /// </summary>
        public EumRuntimeEnv RuntimeEnvType { get; set; }
    }
}