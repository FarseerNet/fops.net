using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.MetaInfoServer.Project.Dal
{
    [Map(typeof(ProjectVO))]
    public class ProjectPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true,IsDbGenerated = true)]
        public int? Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [Field(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 项目组ID
        /// </summary>
        [Field(Name = "group_id")]
        public int? GroupId { get; set; }
        /// <summary>
        /// GIT
        /// </summary>
        [Field(Name = "git_id")]
        public int? GitId { get; set; }
        /// <summary>
        /// 项目路径
        /// </summary>
        [Field(Name = "path")]
        public string Path { get; set; }
    }
}