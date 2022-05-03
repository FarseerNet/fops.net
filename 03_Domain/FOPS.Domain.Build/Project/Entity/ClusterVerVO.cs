namespace FOPS.Domain.Build.Project.Entity
{
    /// <summary>
    /// 集群镜像版本及部署时间
    /// </summary>
    public class ClusterVerVO
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
        /// 上次部署成功的构建ID
        /// </summary>
        public int BuildSuccessId { get; set; }
        /// <summary>
        /// 上次部署失败时间
        /// </summary>
        public DateTime DeployFailAt { get; set; }
        /// <summary>
        /// 上次部署失败的构建ID
        /// </summary>
        public int BuildFailId { get; set; }
    }
}