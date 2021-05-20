using System.Collections.Generic;
using FOPS.Com.DockerServer.DockerfileTpl.Dal;
using FOPS.Com.DockerServer.Hub.Dal;
using FS.Data;
using FS.Data.Map;

namespace FOPS.Com.DockerServer
{
    /// <summary>
    /// 上下文
    /// </summary>
    public class DockerContext : DbContext<DockerContext>
    {
        public DockerContext() : base("fops")
        {
        }
        public TableSet<DockerHubPO>     DockerHub     { get; set; }
        public TableSet<DockerfileTplPO> DockerfileTpl { get; set; }

        protected override void CreateModelInit(Dictionary<string, SetDataMap> map)
        {
            map["DockerHub"].SetName("docker_hub");
            map["DockerfileTpl"].SetName("docker_file_tpl");
        }
    }
}