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
            var psi = new ProcessStartInfo(cmd, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
                
            };

            var runShellResult = new RunShellResult
            {
                IsError = false,
                Output  = new List<string>()
            };

            using (var proc = Process.Start(psi))
            {
                proc.EnableRaisingEvents = true;
                void ProcOnOutputDataReceived(object sender, DataReceivedEventArgs args)
                {
                    runShellResult.Output.Add(args.Data);
                    // 外部第一时间，处理拿到的消息
                    if (actReceiveOutput != null) actReceiveOutput(args.Data);
                }

                proc.OutputDataReceived += ProcOnOutputDataReceived;
                proc.ErrorDataReceived  += ProcOnOutputDataReceived;
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                //开始读取
                //while (!proc.StandardOutput.EndOfStream)
                //{
                //    var output = await proc.StandardOutput.ReadLineAsync();
                //    runShellResult.Output.Add(output);
//
                //    // 外部第一时间，处理拿到的消息
                //    if (actReceiveOutput != null) actReceiveOutput(output);
                //}
//
                //while (!proc.StandardError.EndOfStream)
                //{
                //    var output = await proc.StandardError.ReadLineAsync();
                //    runShellResult.Output.Add(output);
//
                //    // 外部第一时间，处理拿到的消息
                //    if (actReceiveOutput != null) actReceiveOutput(output);
                //}

                // 等待退出
                proc.WaitForExit();
                runShellResult.IsError = proc.ExitCode != 0;
            }

            return runShellResult;
        }
    }
}