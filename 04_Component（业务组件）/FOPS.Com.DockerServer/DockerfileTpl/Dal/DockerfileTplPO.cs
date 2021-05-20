using FOPS.Abstract.Docker.Entity;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.DockerServer.DockerfileTpl.Dal
{
    [Map(typeof(DockerfileTplVO))]
    public class DockerfileTplPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true,IsDbGenerated = true)]
        public int? Id { get; set; }
        
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