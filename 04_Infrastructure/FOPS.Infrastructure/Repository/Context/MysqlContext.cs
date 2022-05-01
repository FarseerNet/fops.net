﻿using FOPS.Infrastructure.Repository.Admin.Model;
using FOPS.Infrastructure.Repository.Build.Model;
using FOPS.Infrastructure.Repository.Cluster.Model;
using FOPS.Infrastructure.Repository.DockerfileTpl.Model;
using FOPS.Infrastructure.Repository.DockerHub.Model;
using FOPS.Infrastructure.Repository.Git.Model;
using FOPS.Infrastructure.Repository.Project.Model;
using FOPS.Infrastructure.Repository.ProjectGroup.Model;
using FOPS.Infrastructure.Repository.YamlTpl.Model;
using FS.Data;

namespace FOPS.Infrastructure.Repository.Context
{
    /// <summary>
    /// 元信息上下文
    /// </summary>
    public class MysqlContext : DbContext<MysqlContext>
    {
        public MysqlContext() : base("fops") { }
        
        public TableSet<ProjectPO>      Project      { get; set; }
        public TableSet<ProjectGroupPO> ProjectGroup { get; set; }
        public TableSet<GitPO>          Git          { get; set; }
        public TableSet<AdminPO>        Admin        { get; set; }
        public TableSet<YamlTplPO>      YamlTpl      { get; set; }
        public TableSet<ClusterPO>       Cluster       { get; set; }
        public TableSet<DockerHubPO>     DockerHub     { get; set; }
        public TableSet<DockerfileTplPO> DockerfileTpl { get; set; }
        public TableSet<BuildPO>         Build         { get; set; }

        protected override void CreateModelInit()
        {
            ProjectGroup.SetName("basic_project_group");
            Project.SetName("basic_project");
            Git.SetName("basic_git");
            Admin.SetName("admin");
            YamlTpl.SetName("k8s_yaml_tpl");
            Cluster.SetName("k8s_cluster");
            DockerHub.SetName("docker_hub");
            DockerfileTpl.SetName("docker_file_tpl");
            Build.SetName("build");
        }
    }
}