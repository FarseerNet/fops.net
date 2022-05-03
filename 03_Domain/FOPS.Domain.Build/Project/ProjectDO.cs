using FOPS.Domain.Build.Enum;
using FOPS.Domain.Build.Project.Entity;
using FOPS.Domain.Build.Project.Repository;
using Newtonsoft.Json;

namespace FOPS.Domain.Build.Project;

public class ProjectDO
{
    /// <summary>
    ///     主键
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///     项目名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    ///     应用ID（链路追踪）
    /// </summary>
    public string AppId { get; set; }
    /// <summary>
    ///     程序入口名称
    /// </summary>
    public string EntryPoint { get; set; }
    /// <summary>
    ///     程序启动端口
    /// </summary>
    public int EntryPort { get; set; }
    /// <summary>
    ///     访问域名
    /// </summary>
    public string Domain { get; set; }
    /// <summary>
    ///     项目组ID
    /// </summary>
    public int GroupId { get; set; }
    /// <summary>
    ///     GIT
    /// </summary>
    public int GitId { get; set; }
    /// <summary>
    ///     依赖的GIT库（会同时拉取依赖的GIT库）
    /// </summary>
    public List<int> DependentGitIds { get; set; }
    /// <summary>
    ///     DockerHub模板
    /// </summary>
    public int DockerHub { get; set; }
    /// <summary>
    ///     DockerfileTpl模板
    /// </summary>
    public int DockerfileTpl { get; set; }
    /// <summary>
    ///     K8SDeployment模板
    /// </summary>
    public int K8STplDeployment { get; set; }
    /// <summary>
    ///     K8SIngress模板
    /// </summary>
    public int K8STplIngress { get; set; }
    /// <summary>
    ///     K8SService模板
    /// </summary>
    public int K8STplService { get; set; }
    /// <summary>
    ///     K8SConfig模板
    /// </summary>
    public int K8STplConfig { get; set; }
    /// <summary>
    ///     K8S模板自定义变量(K1=V1,K2=V2)
    /// </summary>
    public string K8STplVariable { get; set; }
    /// <summary>
    ///     项目路径
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    ///     镜像版本
    /// </summary>
    public string DockerVer { get; set; }
    /// <summary>
    ///     集群版本
    /// </summary>
    public string ClusterVer { get; set; }
    /// <summary>
    ///     构建方式
    /// </summary>
    public EumBuildType BuildType { get; set; }
    /// <summary>
    ///     Shell脚本
    /// </summary>
    public string ShellScript { get; set; }
    /// <summary>
    ///     K8S负载类型
    /// </summary>
    public EumK8sControllers K8sControllersType { get; set; }
    /// <summary>
    /// 集群版本
    /// </summary>
    public Dictionary<int, ClusterVerVO> DicClusterVer { get; set; }
    /// <summary>
    /// 拉取时间
    /// </summary>
    public DateTime GitPullAt { get; set; }

    /// <summary>
    /// 添加项目
    /// </summary>
    public Task<int> AddAsync()
    {
        var repository = IocManager.GetService<IProjectRepository>();

        if (string.IsNullOrWhiteSpace(Path)) Path = "/";
        else if (!Path.StartsWith("/")) Path      = "/" + Path;

        if (string.IsNullOrWhiteSpace(ShellScript)) ShellScript = "";
        Domain = string.IsNullOrWhiteSpace(Domain) ? "" : Domain.ToLower().Replace("http://", "").Replace("https://", "");

        ClusterVer = DicClusterVer != null ? JsonConvert.SerializeObject(DicClusterVer) : "{}";

        return repository.AddAsync(this);
    }

    /// <summary>
    /// 修改项目
    /// </summary>
    public Task UpdateAsync()
    {
        var repository = IocManager.GetService<IProjectRepository>();

        if (string.IsNullOrWhiteSpace(Path)) Path = "/";
        else if (!Path.StartsWith("/")) Path      = "/" + Path;

        if (string.IsNullOrWhiteSpace(ShellScript)) ShellScript = "";
        Domain = string.IsNullOrWhiteSpace(Domain) ? "" : Domain.ToLower().Replace("http://", "").Replace("https://", "");

        ClusterVer = DicClusterVer != null ? JsonConvert.SerializeObject(DicClusterVer) : "{}";

        return repository.UpdateAsync(Id, this);
    }
    
    /// <summary>
    /// 替换模板
    /// </summary>
    public string ReplaceTpl(string tpl)
    {
        if (string.IsNullOrWhiteSpace(tpl)) return null;

        // 替换项目名称
        tpl = tpl.Replace("${project_name}", Name)
                 .Replace("${domain}", Domain)
                 .Replace("${entry_point}", EntryPoint)
                 .Replace("${entry_port}", EntryPort.ToString());

        // 替换模板变量
        foreach (var kv in K8STplVariable.Split(','))
        {
            var kvGroup = kv.Split('=');
            if (kvGroup.Length != 2) continue;
            tpl = tpl.Replace($"${{{kvGroup[0]}}}", kvGroup[1]);
        }

        return tpl;
    }
}