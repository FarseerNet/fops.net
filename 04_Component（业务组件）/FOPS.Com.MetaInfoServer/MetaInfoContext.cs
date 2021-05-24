using System.Collections.Generic;
using FOPS.Com.MetaInfoServer.Admin.Dal;
using FOPS.Com.MetaInfoServer.Git.Dal;
using FOPS.Com.MetaInfoServer.Project.Dal;
using FOPS.Com.MetaInfoServer.ProjectGroup.Dal;
using FS.Data;
using FS.Data.Map;

namespace FOPS.Com.MetaInfoServer
{
    /// <summary>
    /// 元信息上下文
    /// </summary>
    public class MetaInfoContext : DbContext<MetaInfoContext>
    {
        public MetaInfoContext() : base("fops")
        {
        }
        
        public TableSet<ProjectPO>      Project      { get; set; }
        public TableSet<ProjectGroupPO> ProjectGroup { get; set; }
        public TableSet<GitPO>          Git          { get; set; }
        public TableSet<AdminPO>        Admin      { get; set; }

        protected override void CreateModelInit(Dictionary<string, SetDataMap> map)
        {
            map["ProjectGroup"].SetName("basic_project_group");
            map["Project"].SetName("basic_project");
            map["Git"].SetName("basic_git");
            map["Admin"].SetName("admin");
        }
    }
}