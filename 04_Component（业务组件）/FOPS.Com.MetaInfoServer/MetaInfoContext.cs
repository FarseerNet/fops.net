using System.Collections.Generic;
using FOPS.Com.MetaInfoServer.Project.Dal;
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
        
        public TableSet<ProjectPO>   Project { get; set; }

        protected override void CreateModelInit(Dictionary<string, SetDataMap> map)
        {
            map["Project"].SetName("basic_project");
        }
    }
}