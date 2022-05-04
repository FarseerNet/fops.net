namespace FOPS.Domain.Build.Deploy.Device;

public interface ILogWriteDevice: ISingletonDependency
{
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
    /// <summary>
    /// 已完成写入，停止遍历
    /// </summary>
    void Stop(int buildId);
}