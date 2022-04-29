using System;
using System.Collections.Generic;
using FOPS.Abstract.MetaInfo.Enum;

namespace FOPS.Abstract.MetaInfo.Entity
{
    public class ProjectVO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 应用ID（链路追踪）
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 程序入口名
        /// </summary>
        public string EntryPoint { get; set; }
        /// <summary>
        /// 程序启动端口
        /// </summary>
        public int EntryPort { get; set; }
        /// <summary>
        /// 访问域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 项目组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// GIT
        /// </summary>
        public int GitId { get; set; }
        /// <summary>
        /// 依赖的GIT库（会同时拉取依赖的GIT库）
        /// </summary>
        public List<int> DependentGitIds { get; set; }
        /// <summary>
        /// 拉取时间
        /// </summary>
        public DateTime GitPullAt { get; set; }
        /// <summary>
        /// DockerHub模板
        /// </summary>
        public int DockerHub { get; set; }
        /// <summary>
        /// DockerfileTpl模板
        /// </summary>
        public int DockerfileTpl { get; set; }
        /// <summary>
        /// K8SDeployment模板
        /// </summary>
        public int K8STplDeployment { get; set; }
        /// <summary>
        /// K8SIngress模板
        /// </summary>
        public int K8STplIngress { get; set; }
        /// <summary>
        /// K8SService模板
        /// </summary>
        public int K8STplService { get; set; }
        /// <summary>
        /// K8SConfig模板
        /// </summary>
        public int K8STplConfig { get; set; }
        /// <summary>
        /// K8S模板自定义变量(K1=V1,K2=V2)
        /// </summary>
        public string K8STplVariable { get; set; }
        /// <summary>
        /// 项目路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 镜像版本
        /// </summary>
        public string DockerVer { get; set; }
        /// <summary>
        /// 构建方式
        /// </summary>
        public EumBuildType BuildType { get; set; }
        /// <summary>
        /// Shell脚本
        /// </summary>
        public string ShellScript { get; set; }
        /// <summary>
        /// 集群版本
        /// </summary>
        public Dictionary<int,ClusterVer> DicClusterVer { get; set; }
    }
}