using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;

namespace FOPS.Com.BuilderServer.Dotnet
{
    public class DotnetOpr : IDotnetOpr
    {
        public IGitOpr GitOpr { get; set; }

        /// <summary>
        /// 获取项目源地址
        /// </summary>
        public string GetSourceDirRoot(BuildEnvironment env, ProjectVO project, GitVO git) => GitOpr.GetGitPath(env, git) + (project.Path.StartsWith("/") ? project.Path.Substring(1) : project.Path);

        /// <summary>
        /// 获取编译保存的目录地址
        /// </summary>
        public string GetReleasePath(BuildEnvironment env, ProjectVO project) => env.ReleaseDirRoot + project.Name;
    }
}