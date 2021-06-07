using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.Docker.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Infrastructure.Common;
using FS.Core.Entity;
using FS.DI;
using FS.Utils.Component;

namespace FOPS.Com.BuilderServer.Docker
{
    /// <summary>
    /// Docker打包
    /// </summary>
    public class DockerBuildStep:IBuildStep
    {
        public IBuildLogService      BuildLogService      { get; set; }
        public IDockerfileTplService DockerfileTplService { get; set; }
        
        /// <summary>
        /// 构建
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始镜像打包。");

            // Dockerfile不存在，则根据模板创建
            if (!System.IO.File.Exists(env.DockerFilePath))
            {
                BuildLogService.Write(build.Id, $"未发现Dockerfile，将按模板创建");
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
                // 判断目录是否存在（dotnet publish、不编译选项，都实现了创建）
                //if (!System.IO.Directory.Exists(env.ProjectReleaseDirRoot)) System.IO.Directory.CreateDirectory(env.ProjectReleaseDirRoot);
                
                // 替换模板
                var tpl = TplTools.ReplaceTpl(project, dockerfileTpl.Template);
                System.IO.File.AppendAllText(env.DockerFilePath, tpl);
            }
            else
            {
                BuildLogService.Write(build.Id, $"项目中包含Dockerfile，将按存在的Dockerfile进行打包");
            }

            // 打包
            var result = await ShellTools.Run("docker", $"build -t {env.DockerImage} --network=host .", actReceiveOutput, env, env.ProjectReleaseDirRoot);

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
    }
}