namespace FOPS.Domain.Build.Deploy.Device;

public interface IBuildLogDevice: ISingletonDependency
{
    /// <summary>
    /// 写入构建日志
    /// </summary>
    void Write(int buildId, string log);
    /// <summary>
    /// 清除历史记录（正常不会存在，当buildId被重置时，有可能会冲突）
    /// </summary>
    void Clear(int buildId);
    /// <summary>
    /// 查看日志
    /// </summary>
    string[] View(int buildId);
    /// <summary>
    /// 生成Progress类，并自动输出日志
    /// </summary>
    IProgress<string> CreateProgress(int buildId);
}