using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;

namespace FOPS.Com.BuilderServer.Dotnet
{
    public class DotnetOpr : IDotnetOpr
    {
        const string SavePath = "/var/lib/fops/dist/";

        public IGitOpr          GitOpr          { get; set; }

        /// <summary>
        /// 获取项目源地址
        /// </summary>
        public string GetSourceDirRoot(ProjectVO project, GitVO git) => GitOpr.GetGitPath(git) + (project.Path.StartsWith("/") ? project.Path.Substring(1) : project.Path);

        /// <summary>
        /// 获取编译保存的目录地址
        /// </summary>
        public string GetReleasePath(ProjectVO project) => SavePath + project.Name;
    }
}