using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;

namespace FOPS.Infrastructure.Common
{
    public class ShellTools
    {
        /// <summary>
        /// 执行shell 命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<RunShellResult> Run(string cmd, string arguments)
        {
            var psi = new ProcessStartInfo(cmd, arguments) {RedirectStandardOutput = true, RedirectStandardError = true};

            var runShellResult = new RunShellResult
            {
                IsError = false,
                Output  = new List<string>()
            };

            using (var proc = Process.Start(psi))
            {
                if (proc == null) throw new Exception("执行失败，请检查是否正确安装了kubectl工具");

                //开始读取
                while (!proc.StandardOutput.EndOfStream)
                {
                    runShellResult.Output.Add(await proc.StandardOutput.ReadLineAsync());
                }

                while (!proc.StandardError.EndOfStream)
                {
                    runShellResult.Output.Add(await proc.StandardError.ReadLineAsync());
                    runShellResult.IsError = true;
                }
            }

            return runShellResult;
        }
    }
}