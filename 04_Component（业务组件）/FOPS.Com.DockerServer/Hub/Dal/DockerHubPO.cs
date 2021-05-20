using System;
using FOPS.Abstract.Docker.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Mapping.Attribute;
using FS.Mapper;

namespace FOPS.Com.DockerServer.Hub.Dal
{
    [Map(typeof(DockerHubVO))]
    public class DockerHubPO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Field(Name = "id",IsPrimaryKey = true,IsDbGenerated = true)]
        public int? Id { get; set; }
        /// <summary>
        /// 仓库名称
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
        [Field(Name = "user_pwd")]
        public string UserPwd { get; set; }
    }
}