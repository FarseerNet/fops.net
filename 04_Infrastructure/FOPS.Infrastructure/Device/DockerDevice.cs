using FOPS.Domain.Build.Deploy.Device;
using FOPS.Domain.Build.Deploy.Entity;
using FS.Utils.Component;

namespace FOPS.Infrastructure.Device
{
    public class DockerDevice : IDockerDevice
    {
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
        /// <param name="dockerfilePath">dockerfile文件地址</param>
        /// <param name="dockerfileContent">文件内容</param>
        /// <param name="cancellationToken"></param>
        public Task CreateDockerfileAsync(string dockerfilePath, string dockerfileContent, CancellationToken cancellationToken = default)
        {
            if (ExistsDockerfile(dockerfilePath)) return Task.FromResult(0);
            return File.AppendAllTextAsync(dockerfilePath, dockerfileContent, cancellationToken);
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
            var result = await ShellTools.Run("docker", $"build -t {env.DockerImage} --network=host .", receiveOutput, env, env.ProjectReleaseDirRoot, cancellationToken);

            receiveOutput.Report(result == 0 ? $"镜像打包完成。" : $"镜像打包出错了。");
            return result == 0;
        }

        /// <summary>
        /// 上传镜像
        /// </summary>
        public async Task<bool> Push(BuildEnvironment env, IProgress<string> receiveOutput, CancellationToken cancellationToken)
        {
            receiveOutput.Report("---------------------------------------------------------");
            receiveOutput.Report($"开始上传镜像。");

            // 上传
            var result = await ShellTools.Run("docker", $"push {env.DockerImage}", receiveOutput, env, null, cancellationToken);

            receiveOutput.Report(result == 0 ? $"镜像上传完成。" : $"镜像上传出错了。");

            return result == 0;
        }
    }
}