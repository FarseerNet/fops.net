using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Entity;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Infrastructure.Common;
using FS.Core.Entity;
using FS.DI;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Docker
{
    public class DockerOpr : IDockerOpr
    {
        public IBuildLogService      BuildLogService      { get; set; }
        public IIocManager           IocManager           { get; set; }
        public IDockerfileTplService DockerfileTplService { get; set; }
        public IDockerHubService     DockerHubService     { get; set; }

        /// <summary>
        /// 取得dockerHub
        /// </summary>
        public string GetDockerHub(DockerHubVO docker)
        {
            var dockerHub = "localhost";
            if (docker != null)
            {
                dockerHub = docker.Hub;
                if (dockerHub.EndsWith("/")) dockerHub.Substring(0, dockerHub.Length - 1);
            }

            return dockerHub;
        }
        
        /// <summary>
        /// Docker打包
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始镜像打包。");
            // Dockerfile
            var dockerfileTpl = await DockerfileTplService.ToInfoAsync(project.DockerfileTpl);
            if (dockerfileTpl == null)
            {
                var log = $"DockerfileTpl={project.DockerfileTpl}，不存在";
                BuildLogService.Write(build.Id, log);
                return new RunShellResult
                {
                    IsError = true,
                    Output  = new List<string> {log}
                };
            }

            // 如果已存在Dockerfile，则根据模板创建
            if (!System.IO.File.Exists(env.DockerFilePath))
            {
                BuildLogService.Write(build.Id, $"未发现Dockerfile，将按模板创建");
                // 替换模板
                var tpl = TplTools.ReplaceTpl(project, dockerfileTpl.Template);
                System.IO.File.AppendAllText(env.DockerFilePath, tpl);
            }
            else
            {
                BuildLogService.Write(build.Id, $"项目中包含Dockerfile，将按存在的Dockerfile进行打包");
            }

            // 打包
            var result = await ShellTools.Run("docker", $"build -t {env.DockerHub}:{project.Name}-{build.BuildNumber} --network=host .", actReceiveOutput, env, env.ProjectReleaseDirRoot);

            switch (result.IsError)
            {
                case false:
                    BuildLogService.Write(build.Id, $"镜像打包完成。");
                    break;
                case true:
                    BuildLogService.Write(build.Id, $"镜像打包出错了。");
                    break;
            }

            return result;
        }

        /// <summary>
        /// 上传镜像
        /// </summary>
        public async Task<RunShellResult> Upload(BuildEnvironment env, BuildVO build, ProjectVO project, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始上传镜像。");
            // Docker仓库，如果配置了，说明需要上传，则镜像名要设置前缀
            var docker = await DockerHubService.ToInfoAsync(project.DockerHub);

            // 登陆 docker
            if (docker != null && !string.IsNullOrWhiteSpace(docker.UserName))
            {
                await ShellTools.Run("docker", $"login {docker.Hub} -u {docker.UserName} -p {docker.UserPwd}", actReceiveOutput, env);
            }
            
            // 上传
            var result = await ShellTools.Run("docker", $"push {env.DockerHub}:{project.Name}-{build.BuildNumber}", actReceiveOutput, env);

            switch (result.IsError)
            {
                case false:
                    BuildLogService.Write(build.Id, $"镜像上传完成。");
                    break;
                case true:
                    BuildLogService.Write(build.Id, $"镜像上传出错了。");
                    break;
            }

            return result;
        }
    }
}