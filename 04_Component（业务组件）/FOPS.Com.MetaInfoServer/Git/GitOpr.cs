using System;
using System.Threading.Tasks;
using FOPS.Abstract.K8S.Entity;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FOPS.Infrastructure.Common;

namespace FOPS.Com.MetaInfoServer.Git
{
    public class GitOpr : IGitOpr
    {
        public IGitService GitService { get; set; }

        /// <summary>
        /// 拉取最新代码
        /// </summary>
        public async Task<RunShellResult> PullAsync(int gitId)
        {
            var            info = await GitService.ToInfoAsync(gitId);
            //var            path = "/var/lib/fops/git/";
            var            path = "/Users/steden/testfops/fops/git/";
            RunShellResult runShellResult;

            // 判断目录是否存在
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

            // 判断git是否有clone过
            path += info.Id + "/";
            if (!System.IO.Directory.Exists(path))
            {
                runShellResult = await CloneAsync(info);
            }
            else
            {
                runShellResult = await ShellTools.Run("git", $"-C {path} pull");
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
        /// <param name="info"></param>
        /// <returns></returns>
        private async Task<RunShellResult> CloneAsync(GitVO info)
        {
            RunShellResult runShellResult;
            var            url = info.Hub;
            // 需要密码
            if (!string.IsNullOrWhiteSpace(info.UserPwd))
            {
                url = url.Replace("//", $"//{info.UserName.Replace("@", "%40")}:{info.UserPwd}@");
            }

            return await ShellTools.Run("git", $"clone -b {info.Branch} {url} {info.Id}");
        }
    }
}