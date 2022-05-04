using FOPS.Domain.Build.Deploy.Entity;

namespace FOPS.Domain.Build;

/// <summary>
/// 检查目录
/// </summary>
public class CheckDirectoryService : ISingletonDependency
{
    public void Check(BuildEnvironment env, IProgress<string> progress, CancellationToken cancellationToken)
    {
        progress.Report("---------------------------------------------------------");
        progress.Report($"前置检查。");

        // 先删除之前编译的目标文件
        progress.Report($"先删除之前编译的目标文件。");
        if (Directory.Exists(BuildEnvironment.DistRoot))
        {
            //获取指定路径下所有文件夹
            foreach (string folderPath in Directory.GetDirectories(BuildEnvironment.DistRoot))
                Directory.Delete(folderPath, true);
            
            //获取指定路径下所有文件
            foreach (string filePath in Directory.GetFiles(BuildEnvironment.DistRoot))
                File.Delete(filePath);
            
            // k8s中，BuildEnvironment.DistRoot路径是挂载的，因此无法删除该文件夹，只能使用上面的清理文件的方式
            //Directory.Delete(BuildEnvironment.DistRoot, true);
        }
        
        // 自动创建目录
        progress.Report($"自动创建目录。");
        if (!Directory.Exists(BuildEnvironment.FopsRoot)) Directory.CreateDirectory(BuildEnvironment.FopsRoot);
        if (!Directory.Exists(BuildEnvironment.NpmModulesRoot)) Directory.CreateDirectory(BuildEnvironment.NpmModulesRoot);
        if (!Directory.Exists(BuildEnvironment.DistRoot)) Directory.CreateDirectory(BuildEnvironment.DistRoot);
        if (!Directory.Exists(BuildEnvironment.KubeRoot)) Directory.CreateDirectory(BuildEnvironment.KubeRoot);
        if (!Directory.Exists(BuildEnvironment.ShellRoot)) Directory.CreateDirectory(BuildEnvironment.ShellRoot);
        if (!Directory.Exists(BuildEnvironment.GitRoot)) Directory.CreateDirectory(BuildEnvironment.GitRoot);

        //Directory.CreateDirectory(env.ProjectDistRoot);

        progress.Report($"前置检查通过。");
    }
}