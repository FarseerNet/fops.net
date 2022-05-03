using System.Collections.Concurrent;
using System.Text;
using FOPS.Domain.Build.Deploy.Device;
using FS.Extends;
using Microsoft.Extensions.Logging;

namespace FOPS.Infrastructure.Device;

/// <summary>
/// 构建时的日志输出
/// </summary>
public class BuildLogDevice : IBuildLogDevice
{
    const  string      SavePath = "/var/lib/fops/log/";
    public IIocManager IocManager { get; set; }

    private readonly Dictionary<int, ConcurrentQueue<string>> QueueLog = new();
    private readonly Dictionary<int, bool>                    _runing  = new();

    /// <summary>
    /// 生成Progress类，并自动输出日志
    /// </summary>
    public IProgress<string> CreateProgress(int buildId)
    {
        var logfile = SavePath + $"{buildId}.txt";
        if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
        using (File.Create(logfile)) { }

        QueueLog.TryAdd(buildId, new ConcurrentQueue<string>());
        _runing.TryAdd(buildId, true);
        TimingWrite(buildId);

        return new Progress<string>(log =>
        {
            IocManager.Logger<BuildLogDevice>().LogInformation($"构建任务id={buildId}：{log}。");
            QueueLog[buildId].Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {log}");
        });
    }

    /// <summary>
    /// 写入构建日志
    /// </summary>
    private void Write(int buildId, string log)
    {
        var logfile = SavePath + $"{buildId}.txt";
        File.AppendAllText(logfile, $"{log}\r\n", Encoding.UTF8);
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

    /// <summary>
    /// 定时写入文件
    /// </summary>
    private void TimingWrite(int buildId)
    {
        Task.Run(() =>
        {
            while (_runing[buildId])
            {
                var lst = QueueLog[buildId].DequeueAll();
                if (lst.Count > 0) Write(buildId, string.Join("\r\n", lst));
                Thread.Sleep(500);
            }

            QueueLog.Remove(buildId);
            _runing.Remove(buildId);
        });
    }

    /// <summary>
    /// 已完成写入，停止遍历
    /// </summary>
    public void Stop(int buildId)
    {
        _runing[buildId] = false;
        var lst = QueueLog[buildId].DequeueAll();
        if (lst.Count > 0) Write(buildId, string.Join("\r\n", lst));
    }
}