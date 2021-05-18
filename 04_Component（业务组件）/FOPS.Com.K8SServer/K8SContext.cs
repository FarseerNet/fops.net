﻿using System.Collections.Generic;
using FOPS.Com.K8sServerAA.YamlTpl.Dal;
using FS.Data;
using FS.Data.Map;

namespace FOPS.Com.K8sServerAA
{
    /// <summary>
    /// K8S上下文
    /// </summary>
    public class K8SContext : DbContext<K8SContext>
    {
        public K8SContext() : base("fops")
        {
        }
        public TableSet<YamlTplPO> YamlTpl { get; set; }

        protected override void CreateModelInit(Dictionary<string, SetDataMap> map)
        {
            map["YamlTpl"].SetName("k8s_yaml_tpl");
        }
    }
}