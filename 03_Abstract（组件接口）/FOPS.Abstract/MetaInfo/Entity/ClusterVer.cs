using System;

namespace FOPS.Abstract.MetaInfo.Entity
{
    /// <summary>
    /// 集群镜像版本及部署时间
    /// </summary>
    public class ClusterVer
    {
        /// <summary>
        /// 集群镜像版本
        /// </summary>
        public string DockerVer { get; set; }
        /// <summary>
        /// 上次部署成功时间
        /// </summary>
        public DateTime DeploySuccessAt { get; set; }
        /// <summary>
        /// 上次部署失败时间
        /// </summary>
        public DateTime DeployFailAt { get; set; }
    }
}