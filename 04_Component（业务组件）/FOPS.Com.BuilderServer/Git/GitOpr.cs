using System;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Infrastructure.Common;

namespace FOPS.Com.BuilderServer.Git
{
    public class GitOpr : IGitOpr
    {
        public IGitService GitService { get; set; }
        const  string      SavePath = "/var/lib/fops/git/";

        /// <summary>
        /// 拉取最新代码
        /// </summary>
        public async Task<RunShellResult> PullAsync(int gitId)
        {
            var            info = await GitService.ToInfoAsync(gitId);
            RunShellResult runShellResult;

            // 判断目录是否存在
            if (!System.IO.Directory.Exists(SavePath)) System.IO.Directory.CreateDirectory(SavePath);

            // 判断git是否有clone过
            var gitPath = SavePath + info.Id + "/";
            if (!System.IO.Directory.Exists(SavePath))
            {
                runShellResult = await CloneAsync(info, gitPath);
            }
            else
            {
                runShellResult = await ShellTools.Run("git", $"-C {gitPath} pull");
            }

            if (!runShellResult.IsError)
            {
                info        = await GitService.ToInfoAsync(gitId);
                info.PullAt = DateTime.Now;
                await GitService.UpdateAsync(gitId, info);
            }

            return runShellResult;
        }

        /// <summary>
        /// Clone代码
        /// </summary>
        private async Task<RunShellResult> CloneAsync(GitVO info, string path)
        {
            var url = info.Hub;
            // 需要密码
            if (!string.IsNullOrWhiteSpace(info.UserPwd))
            {
                url = url.Replace("//", $"//{info.UserName.Replace("@", "%40")}:{info.UserPwd}@");
            }

            return await ShellTools.Run("git", $"clone -b {info.Branch} {url} {path}");
        }
    }
}