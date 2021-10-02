using System.Collections.Generic;
using FOPS.Com.FssServer.TaskGroup.Dal;
using FOPS.Com.FssServer.Tasks.Dal;
using FS.Data;
using FS.Data.Map;

namespace FOPS.Com.FssServer
{
    /// <summary>
    /// 元信息上下文
    /// </summary>
    public class MetaInfoContext : DbContext<MetaInfoContext>
    {
        public MetaInfoContext() : base("fss_db")
        {
        }
        
        public TableSet<TaskPO>      Task      { get; set; }
        public TableSet<TaskGroupPO> TaskGroup { get; set; }

        protected override void CreateModelInit(Dictionary<string, SetDataMap> map)
        {
            map["Task"].SetName("task");
            map["TaskGroup"].SetName("task_group");
        }
    }
}