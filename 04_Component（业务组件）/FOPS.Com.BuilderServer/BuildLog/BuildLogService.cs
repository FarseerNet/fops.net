using System;
using System.Collections.Generic;
using FOPS.Abstract.Builder.Server;
using FOPS.Com.BuilderServer.Build;
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
            System.IO.File.AppendAllText(path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {log}\r\n");

            IocManager.Logger<BuildLogService>().LogDebug($"构建任务id={buildId}：{log}。");
        }

        /// <summary>
        /// 查看日志
        /// </summary>
        public string[] View(int buildId)
        {
            var path = SavePath + $"{buildId}.txt";
            return !System.IO.File.Exists(path) ? new string[0] : System.IO.File.ReadAllLines(path);
        }
    }
}