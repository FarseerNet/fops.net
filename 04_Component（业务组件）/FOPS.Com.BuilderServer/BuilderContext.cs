using System.Collections.Generic;
using FOPS.Com.BuilderServer.Build.Dal;
using FS.Data;
using FS.Data.Map;

namespace FOPS.Com.BuilderServer
{
    /// <summary>
    /// 上下文
    /// </summary>
    public class BuilderContext : DbContext<BuilderContext>
    {
        public BuilderContext() : base("fops")
        {
        }

        protected override void CreateModelInit(Dictionary<string, SetDataMap> map)
        {
            map["Build"].SetName("build");
        }
        
        public TableSet<BuildPO> Build { get; set; }
    }
}