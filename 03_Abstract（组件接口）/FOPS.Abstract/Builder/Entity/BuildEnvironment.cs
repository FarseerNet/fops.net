using System.Collections.Generic;

namespace FOPS.Abstract.Builder.Entity
{
    /// <summary>
    /// 构建时的环境变量
    /// </summary>
    public class BuildEnvironment
    {
        /// <summary>
        /// 构建版本号
        /// </summary>
        public int BuildNumber { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目访问域名
        /// </summary>
        public string ProjectDomain { get; set; }

        /// <summary>
        /// 程序入口名
        /// </summary>
        public string ProjectEntryPoint { get; set; }

        /// <summary>
        /// 程序启动端口
        /// </summary>
        public int ProjectEntryPort { get; set; }

        /// <summary>
        /// 项目编译保存的目录
        /// </summary>
        public string ProjectReleaseDirRoot { get; set; }

        /// <summary>
        /// Fops根目录
        /// </summary>
        public string FopsDirRoot => "/var/lib/fops/";

        /// <summary>
        /// NpmModules
        /// </summary>
        public string NpmModulesDirRoot => "/var/lib/fops/npm";

        /// <summary>
        /// 编译保存的根目录
        /// </summary>
        public string ReleaseDirRoot => "/var/lib/fops/dist/";

        /// <summary>
        /// GIT根目录
        /// </summary>
        public string GitDirRoot => "/var/lib/fops/git/";

        /// <summary>
        /// kubectlConfig配置
        /// </summary>
        public string KubePath => "/var/lib/fops/kube/";

        /// <summary>
        /// 生成Shell脚本的存放路径
        /// </summary>
        public string ShellScriptPath => "/var/lib/fops/shell/";

        /// <summary>
        /// 项目源代码目录
        /// </summary>
        public string ProjectSourceDirRoot { get; set; }

        /// <summary>
        /// Git仓库地址
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// Git仓库源代码目录
        /// </summary>
        public string ProjectGitDirRoot { get; set; }

        /// <summary>
        /// Dockerfile文件路径
        /// </summary>
        public string DockerFilePath { get; set; }

        /// <summary>
        /// Docker仓库地址
        /// </summary>
        public string DockerHub { get; set; }

        /// <summary>
        /// Docker镜像
        /// </summary>
        public string DockerImage { get; set; }

        /// <summary>
        /// 转成字典
        /// </summary>
        public static implicit operator Dictionary<string, string>(BuildEnvironment env)
        {
            return new()
            {
                {"Fops_DirRoot", env.FopsDirRoot},
                {"NpmModules_DirRoot", env.NpmModulesDirRoot},
                {"Git_Hub", env.GitHub},
                {"Git_DirRoot", env.GitDirRoot},
                {"Build_Number", env.BuildNumber.ToString()},
                {"Project_Name", env.ProjectName},
                {"Project_Domain", env.ProjectDomain},
                {"Project_GitDirRoot", env.ProjectGitDirRoot},
                {"Project_EntryPoint", env.ProjectEntryPoint},
                {"Project_EntryPort", env.ProjectEntryPort.ToString()},
                {"Project_ReleaseDirRoot", env.ProjectReleaseDirRoot},
                {"Project_SourceDirRoot", env.ProjectSourceDirRoot},
                {"Docker_FilePath", env.DockerFilePath},
                {"Docker_Hub", env.DockerHub},
                {"Docker_Image", env.DockerImage},
                {"ReleaseDirRoot", env.ReleaseDirRoot},
                {"Kube_Path", env.KubePath},
            };
        }
    }
}