namespace FOPS.Abstract.MetaInfo.Entity
{
    public class ProjectGroupVO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 集群ID
        /// </summary>
        public int ClusterId { get; set; }
        /// <summary>
        /// 项目组名称
        /// </summary>
        public string Name { get; set; }
    }
}