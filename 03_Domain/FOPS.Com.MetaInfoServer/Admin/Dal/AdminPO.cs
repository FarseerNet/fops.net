using System;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.MetaInfoServer.Admin.Dal
{
    [Map(typeof(AdminVO))]
    public class AdminPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true,IsDbGenerated = true)]
        public int? Id { get; set; }
        /// <summary>
        /// 管理员名称
        /// </summary>
        [Field(Name = "user_name")]
        public string UserName { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        [Field(Name = "user_pwd")]
        public string UserPwd { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Field(Name = "is_enable")]
        public bool? IsEnable { get; set; }
        /// <summary>
        /// 上次登陆时间
        /// </summary>
        [Field(Name = "last_login_at")]
        public DateTime? LastLoginAt { get; set; }
        /// <summary>
        /// 上次登陆IP
        /// </summary>
        [Field(Name = "last_login_ip")]
        public string LastLoginIp { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Field(Name = "create_at")]
        public DateTime? CreateAt { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Field(Name = "create_user")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        [Field(Name = "create_id")]
        public string CreateId { get; set; }
    }
}