namespace FOPS.Abstract.Docker.Entity
{
    public class DockerHubVO
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 托管地址
        /// </summary>
        public string Hub { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 账户密码
        /// </summary>
        public string UserPwd { get; set; }
    }
}