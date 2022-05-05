using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FS.Utils.Component;

namespace FOPS.Infrastructure.Device
{
    public class DockerDevice : IDockerDevice
    {
        // /// <summary>
        // /// 生成Dockerfile路径
        // /// </summary>
        // public string GetDockerfilePath(string projectName) => $"{BuildEnvironment.DockerfilePath}{projectName}";
        //
        /// <summary>
        /// 取得dockerHub
        /// </summary>
        public string GetDockerHub(string dockerHubAddress)
        {
            var dockerHub = "localhost";
            if (!string.IsNullOrWhiteSpace(dockerHubAddress))
            {
                dockerHub = dockerHubAddress;
                if (dockerHub.EndsWith("/")) dockerHub.Substring(0, dockerHub.Length - 1);
            }

            return dockerHub;
        }

        /// <summary>
        /// 生成镜像名称
        /// </summary>
        public string GetDockerImage(string dockerHubAddress, string projectName, int buildNumber) => $"{GetDockerHub(dockerHubAddress)}:{projectName}-{buildNumber}";

        /// <summary>
        /// 生成Dockerfile文件
        /// </summary>
        /// <param name="projectName">dockerfile文件地址</param>
        /// <param name="dockerfileContent">文件内容</param>
        /// <param name="cancellationToken"></param>
        public async Task CreateDockerfileAsync(string projectName, string dockerfileContent, CancellationToken cancellationToken = default)
        {
            if (File.Exists(BuildEnvironment.DockerfilePath)) File.Delete(BuildEnvironment.DockerfilePath);

            var ignoreContent = @"**/.dockerignore
**/.env
**/.git
**/.gitignore
**/.project
**/.settings
**/.toolstarget
**/.vs
**/.vscode
**/.idea
**/*.*proj.user
**/*.dbmdl
**/*.jfm
**/azds.yaml
**/bin
**/charts
**/docker-compose*
**/Dockerfile*
**/node_modules
**/npm-debug.log
**/obj
**/secrets.dev.yaml
**/values.dev.yaml
LICENSE
README.md
";
            //await File.AppendAllTextAsync(BuildEnvironment.DockerIgnorePath, ignoreContent, cancellationToken);
            await File.AppendAllTextAsync(BuildEnvironment.DockerfilePath, dockerfileContent, cancellationToken);
        }

        /// <summary>
        /// dockerfile文件是否存在
        /// </summary>
        public bool ExistsDockerfile(string dockerfilePath) => File.Exists(dockerfilePath);

        /// <summary>
        /// 登陆容器仓库
        /// </summary>
        public async Task<bool> LoginAsync(string dockerHub, string loginName, string loginPwd, IProgress<string> receiveOutput, BuildEnvironment env, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(dockerHub) && !string.IsNullOrWhiteSpace(loginName))
            {
                var result = await ShellTools.Run("docker", $"login {dockerHub} -u {loginName} -p {loginPwd}", receiveOutput, env, null, cancellationToken);
                if (result != 0)
                {
                    receiveOutput.Report("镜像仓库登陆失败。");
                    return false;
                }
            }

            receiveOutput.Report("登陆成功。");
            return true;
        }

        /// <summary>
        /// 容器构建
        /// </summary>
        public async Task<bool> Build(BuildEnvironment env, IProgress<string> receiveOutput, CancellationToken cancellationToken)
        {
            // 打包
            var result = await ShellTools.Run("docker", $"build -t {env.DockerImage} --network=host -f {BuildEnvironment.DockerfilePath} {BuildEnvironment.DistRoot}", receiveOutput, env, BuildEnvironment.DistRoot, cancellationToken);

            receiveOutput.Report(result == 0 ? $"镜像打包完成。" : $"镜像打包出错了。");
            return result == 0;
        }

        /// <summary>
        /// 上传镜像
        /// </summary>
        public async Task<bool> Push(BuildEnvironment env, IProgress<string> receiveOutput, CancellationToken cancellationToken)
        {
            // 上传
            var result = await ShellTools.Run("docker", $"push {env.DockerImage}", receiveOutput, env, null, cancellationToken);

            receiveOutput.Report(result == 0 ? $"镜像上传完成。" : $"镜像上传出错了。");

            return result == 0;
        }
    }
}