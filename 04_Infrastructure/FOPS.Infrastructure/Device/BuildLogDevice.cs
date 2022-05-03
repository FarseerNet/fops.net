using System.Text;
using FOPS.Domain.Build.Deploy.Device;
using Microsoft.Extensions.Logging;

namespace FOPS.Infrastructure.Device;

/// <summary>
/// 构建时的日志输出
/// </summary>
public class BuildLogDevice : IBuildLogDevice
{
    const  string      SavePath = "/var/lib/fops/log/";
    public IIocManager IocManager { get; set; }

    /// <summary>
    /// 生成Progress类，并自动输出日志
    /// </summary>
    public IProgress<string> CreateProgress(int buildId)
    {
        return new Progress<string>(log =>
        {
            Write(buildId, log);
        });
    }
    
    /// <summary>
    /// 写入构建日志
    /// </summary>
    public void Write(int buildId, string log)
    {
        var path = SavePath + $"{buildId}.txt";
        if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
        File.AppendAllText(path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {log}\r\n", Encoding.UTF8);

        IocManager.Logger<BuildLogDevice>().LogDebug($"构建任务id={buildId}：{log}。");
    }

    /// <summary>
    /// 清除历史记录（正常不会存在，当buildId被重置时，有可能会冲突）
    /// </summary>
    public void Clear(int buildId)
    {
        var path = SavePath + $"{buildId}.txt";
        if (File.Exists(path)) File.Delete(path);
    }

    /// <summary>
    /// 查看日志
    /// </summary>
    public string[] View(int buildId)
    {
        var path = SavePath + $"{buildId}.txt";
        return !File.Exists(path) ? Array.Empty<string>() : File.ReadAllLines(path);
    }
}