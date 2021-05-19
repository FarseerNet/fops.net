using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.K8S.Entity;
using FOPS.Infrastructure.Common;

namespace FOPS.Com.BuilderServer.Dotnet
{
    public class DotnetOpr : IDotnetOpr
    {
        const string SavePath = "/var/lib/fops/dist/";
        
        /// <summary>
        /// 编译.net core
        /// </summary>
        public async Task<RunShellResult> Publish(string project,string source,Action<string> actReceiveOutput)
        {
            var projectPath = SavePath + project;
            return await ShellTools.Run("dotnet", $"public -c Release -o {projectPath} {source}", actReceiveOutput);
        }
    }
}