using System;

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
        /// 项目组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// GIT
        /// </summary>
        public int GitId { get; set; }
        /// <summary>
        /// 拉取时间
        /// </summary>
        public DateTime GitPullAt { get; set; }
        /// <summary>
        /// Git名称
        /// </summary>
        public string GitName { get; set; }
        /// <summary>
        /// 托管地址
        /// </summary>
        public string GitHub { get; set; }
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
    }
}