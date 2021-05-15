namespace FOPS.Abstract.MetaInfo.Entity
{
    public class ProjectVO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目组ID
        /// </summary>
        public int? GroupId { get; set; }
        /// <summary>
        /// GIT
        /// </summary>
        public int? GitId { get; set; }
    }
}