using System;
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
    public class DockerUploadStep:IBuildStep
    {
        // public DockerUploadStep(IDockerfileTplService dockerfileTplService)
        // {
        //     DockerfileTplService = dockerfileTplService;
        // }

        public IBuildLogService      BuildLogService      { get; set; }
        public IProjectService       ProjectService       { get; set; }
        public IDockerfileTplService DockerfileTplService { get; set; }
        
        /// <summary>
        /// 构建
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始上传镜像。");
            
            // 上传
            var result = await ShellTools.Run("docker", $"push {env.DockerImage}", actReceiveOutput, env);

            switch (result.IsError)
            {
                case false:
                    // 修改项目的镜像版本
                    project.DockerVer = build.BuildNumber.ToString();
                    await ProjectService.UpdateAsync(project.Id, project.DockerVer);
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