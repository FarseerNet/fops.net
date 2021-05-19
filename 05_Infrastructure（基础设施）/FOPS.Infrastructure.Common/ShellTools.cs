using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
        /// <param name="actReceiveOutput">外部第一时间，处理拿到的消息 </param>
        public static async Task<RunShellResult> Run(string cmd, string arguments, Action<string> actReceiveOutput)
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
                    var output = await proc.StandardOutput.ReadLineAsync();
                    runShellResult.Output.Add(output);

                    // 外部第一时间，处理拿到的消息
                    if (actReceiveOutput != null) actReceiveOutput(output);
                }

                while (!proc.StandardError.EndOfStream)
                {
                    var output = await proc.StandardError.ReadLineAsync();
                    runShellResult.Output.Add(output);

                    // 外部第一时间，处理拿到的消息
                    if (actReceiveOutput != null) actReceiveOutput(output);
                }

                // 等待退出
                while (!proc.HasExited)
                {
                    await Task.Delay(1000);
                }

                runShellResult.IsError = proc.ExitCode != 0;
            }

            return runShellResult;
        }
    }
}