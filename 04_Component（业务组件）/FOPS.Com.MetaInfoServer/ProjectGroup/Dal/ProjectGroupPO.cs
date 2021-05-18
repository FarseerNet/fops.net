using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.MetaInfoServer.ProjectGroup.Dal
{
    [Map(typeof(ProjectGroupVO))]
    public class ProjectGroupPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true,IsDbGenerated = true)]
        public int? Id { get; set; }
        /// <summary>
        /// 集群ID
        /// </summary>
        [Field(Name = "cluster_id")]
        public int? ClusterId { get; set; }
        /// <summary>
        /// 项目组名称
        /// </summary>
        [Field(Name = "name")]
        public string Name { get; set; }
    }
}