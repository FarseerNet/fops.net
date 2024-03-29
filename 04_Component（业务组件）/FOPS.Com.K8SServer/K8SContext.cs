﻿using System.Collections.Generic;
using FOPS.Com.K8SServer.Cluster.Dal;
using FOPS.Com.K8SServer.YamlTpl.Dal;
using FS.Data;
using FS.Data.Map;

namespace FOPS.Com.K8SServer
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
        
        public TableSet<ClusterPO> Cluster { get; set; }

        protected override void CreateModelInit()
        {
            YamlTpl.SetName("k8s_yaml_tpl");
            Cluster.SetName("k8s_cluster");
        }
    }
}