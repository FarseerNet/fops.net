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
    public class DockerUploadStep : IBuildStep
    {
        public IBuildLogService BuildLogService { get; set; }
        public IProjectService  ProjectService  { get; set; }

        /// <summary>
        /// 构建
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildDTO build, ProjectDTO project, GitDTO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            BuildLogService.Write(build.Id, "---------------------------------------------------------");
            BuildLogService.Write(build.Id, $"开始上传镜像。");

            // 上传
            var result = await ShellTools.Run("docker", $"push {env.DockerImage}", actReceiveOutput, env, null, cancellationToken);

            switch (result.IsError)
            {
                case false:
                    // 修改项目的镜像版本
                    project.DockerVer = build.BuildNumber.ToString();
                    await ProjectService.UpdateAsync(project.Id, project.DockerVer);
                    return new RunShellResult(false, "镜像上传完成。");
                case true:
                    return new RunShellResult(true, "镜像上传出错了。");
            }
        }
    }
}