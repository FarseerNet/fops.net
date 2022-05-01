using System;
using System.Threading;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FS.Core.Entity;
using FS.DI;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Docker
{
    /// <summary>
    /// Docker上传镜像
    /// </summary>
    public class DockerLoginStep : IBuildStep
    {
        public IBuildLogService  BuildLogService  { get; set; }
        public IDockerHubService DockerHubService { get; set; }

        /// <summary>
        /// 构建
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildDTO build, ProjectDTO project, GitDTO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"登陆镜像仓库。");

            // Docker仓库，如果配置了，说明需要上传，则镜像名要设置前缀
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);

            // 登陆 docker
            if (docker != null && !string.IsNullOrWhiteSpace(docker.UserName))
            {
                var result = await ShellTools.Run("docker", $"login {docker.Hub} -u {docker.UserName} -p {docker.UserPwd}", actReceiveOutput, env, null, cancellationToken);
                if (result.IsError)
                {
                    return new RunShellResult(true, "镜像仓库登陆失败。");
                }
            }

            // 不需要登陆
            return new RunShellResult(false, "登陆成功。");
        }
    }
}