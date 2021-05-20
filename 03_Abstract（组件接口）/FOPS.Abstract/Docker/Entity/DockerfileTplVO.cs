namespace FOPS.Abstract.Docker.Entity
{
    public class DockerfileTplVO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? Id { get; set; }
        
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