using FOPS.Com.MetaInfoServer.Git.Dal;
using FOPS.Com.MetaInfoServer.Project.Dal;
using FOPS.Com.MetaInfoServer.ProjectGroup.Dal;
using FS.Data;

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

        protected override void CreateModelInit()
        {
            ProjectGroup.SetName("basic_project_group");
            Project.SetName("basic_project");
            Git.SetName("basic_git");
        }
    }
}