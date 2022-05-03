namespace FOPS.Domain.Build.Deploy.Entity
{
    /// <summary>
    /// 构建时的环境变量
    /// </summary>
    public class BuildEnvironment
    {
        /// <summary>
        /// 构建版本号
        /// </summary>
        public int BuildId { get; set; }

        /// <summary>
        /// 构建版本号
        /// </summary>
        public int BuildNumber { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectId { get; set; }

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
        /// Fops根目录
        /// </summary>
        public const string FopsRoot = "/var/lib/fops/";

        /// <summary>
        /// kubectlConfig配置
        /// </summary>
        public const string KubeRoot = "/var/lib/fops/kube/";

        /// <summary>
        /// NpmModules
        /// </summary>
        public const string NpmModulesRoot = "/var/lib/fops/npm";

        /// <summary>
        /// 编译保存的根目录
        /// </summary>
        public const string DistRoot = "/var/lib/fops/dist/";

        /// <summary>
        /// GIT根目录
        /// </summary>
        public const string GitRoot = "/var/lib/fops/git/";

        /// <summary>
        /// Dockerfile根目录
        /// </summary>
        public const string DockerfilePath = "/var/lib/fops/dist/Dockerfile";

        /// <summary>
        /// 生成Shell脚本的存放路径
        /// </summary>
        public const string ShellRoot = "/var/lib/fops/shell/";

        // /// <summary>
        // /// 项目编译保存的目录（包含依赖的框架项目）***************
        // /// /var/lib/fops/dist/{projectName1}/
        // /// /var/lib/fops/dist/{projectName2}/
        // /// /var/lib/fops/dist/{projectName3}/
        // /// </summary>
        // public string[] ProjectDistRoot { get; set; }

        // /// <summary>
        // /// Dockerfile文件路径************************
        // /// /var/lib/fops/dockerfile/{projectName}
        // /// </summary>
        // public string ProjectDockerfilePath { get; set; }
        //
        // /// <summary>
        // /// 项目源代码目录************************
        // /// /var/lib/fops/git/{gitName}/{projectPath}
        // /// </summary>
        // public string ProjectSourceDirRoot { get; set; }

        /// <summary>
        /// Git仓库源代码根目录
        /// /var/lib/fops/git/{gitName}/
        /// </summary>
        public string ProjectGitRoot { get; set; }

        /// <summary>
        /// Git仓库地址
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// Git名称（项目的目录名称）
        /// </summary>
        public string GitName { get; set; }

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
                { "FopsRoot", FopsRoot },
                { "NpmModulesRoot", NpmModulesRoot },
                { "DistRoot", DistRoot },
                { "KubeRoot", KubeRoot },
                { "Git_Root", GitRoot },
                { "Git_Hub", env.GitHub },
                { "Build_Number", env.BuildNumber.ToString() },
                { "Project_Name", env.ProjectName },
                { "Project_Domain", env.ProjectDomain },
                { "Project_GitRoot", env.ProjectGitRoot },
                { "Project_EntryPoint", env.ProjectEntryPoint },
                { "Project_EntryPort", env.ProjectEntryPort.ToString() },
                // { "Project_DistRoot", env.ProjectDistRoot },
                // { "Project_SourceDirRoot", env.ProjectSourceDirRoot },
                // { "Docker_DockerfilePath", env.ProjectDockerfilePath },
                { "Docker_Hub", env.DockerHub },
                { "Docker_Image", env.DockerImage },
                { "Git_Name", env.GitName },
            };
        }
    }
}