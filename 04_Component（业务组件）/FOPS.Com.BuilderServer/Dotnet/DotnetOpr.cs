using System;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Infrastructure.Common;

namespace FOPS.Com.BuilderServer.Dotnet
{
    public class DotnetOpr
    {
        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(Action<string> actReceiveOutput)
        {
            return await ShellTools.Run("dotnet", "public -c Release", actReceiveOutput);
        }
    }
}