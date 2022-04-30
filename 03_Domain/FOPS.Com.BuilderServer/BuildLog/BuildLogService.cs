using System;
using System.Text;
using FOPS.Abstract.Builder.Server;
using FS.DI;
using Microsoft.Extensions.Logging;

namespace FOPS.Com.BuilderServer.BuildLog
{
    public class BuildLogService : IBuildLogService
    {
        const  string      SavePath = "/var/lib/fops/log/";
        public IIocManager IocManager { get; set; }

        /// <summary>
        /// 写入构建日志
        /// </summary>
        public void Write(int buildId, string log)
        {
            var path = SavePath + $"{buildId}.txt";
            if (!System.IO.Directory.Exists(SavePath)) System.IO.Directory.CreateDirectory(SavePath);
            System.IO.File.AppendAllText(path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {log}\r\n", Encoding.UTF8);

            IocManager.Logger<BuildLogService>().LogDebug($"构建任务id={buildId}：{log}。");
        }

        /// <summary>
        /// 清除历史记录（正常不会存在，当buildId被重置时，有可能会冲突）
        /// </summary>
        public void Clear(int buildId)
        {
            var path = SavePath + $"{buildId}.txt";
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }

        /// <summary>
        /// 查看日志
        /// </summary>
        public string[] View(int buildId)
        {
            var path = SavePath + $"{buildId}.txt";
            return !System.IO.File.Exists(path) ? Array.Empty<string>() : System.IO.File.ReadAllLines(path);
        }
    }
}