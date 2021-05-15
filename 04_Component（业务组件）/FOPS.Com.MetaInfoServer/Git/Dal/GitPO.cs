using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.MetaInfoServer.Git.Dal
{
    [Map(typeof(GitVO))]
    public class GitPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true)]
        public int? Id { get; set; }
        /// <summary>
        /// Git名称
        /// </summary>
        [Field(Name = "name")]
        public string Name { get; set; }
        /// <summary>
        /// 托管地址
        /// </summary>
        [Field(Name = "hub")]
        public string Hub { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        [Field(Name = "user_name")]
        public string UserName { get; set; }
        /// <summary>
        /// 账户密码
        /// </summary>
        [Field(Name = "password")]
        public string PassWord { get; set; }
    }
}