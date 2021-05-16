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
        public string ConfigName { get; set; }
    }
}