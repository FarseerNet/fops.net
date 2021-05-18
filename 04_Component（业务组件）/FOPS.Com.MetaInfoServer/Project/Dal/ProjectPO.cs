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
        /// K8SDeployment模板
        /// </summary>
        [Field(Name = "k8s_tpl_deployment")]
        public int? K8STplDeployment { get; set; }
        /// <summary>
        /// K8SIngress模板
        /// </summary>
        [Field(Name = "k8s_tpl_ingress")]
        public int? K8STplIngress { get; set; }
        /// <summary>
        /// K8SService模板
        /// </summary>
        [Field(Name = "k8s_tpl_service")]
        public int? K8STplService { get; set; }
        /// <summary>
        /// K8SConfig模板
        /// </summary>
        [Field(Name = "k8s_tpl_config")]
        public int? K8STplConfig { get; set; }
        /// <summary>
        /// K8S模板自定义变量(K1=V1,K2=V2)
        /// </summary>
        [Field(Name = "k8s_tpl_variable")]
        public string K8STplVariable { get; set; }
        /// <summary>
        /// 项目路径
        /// </summary>
        [Field(Name = "path")]
        public string Path { get; set; }
        /// <summary>
        /// 镜像版本
        /// </summary>
        [Field(Name = "docker_ver")]
        public string DockerVer { get; set; }
        /// <summary>
        /// 集群版本
        /// </summary>
        [Field(Name = "cluster_ver")]
        public string ClusterVer { get; set; }
    }
}