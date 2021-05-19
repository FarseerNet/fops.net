using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Server;
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
        /// 消除仓库
        /// </summary>
        public void Clear(int gitId)
        {
            var varLibFopsGit = SavePath + $"{gitId}/";
            if (System.IO.Directory.Exists(varLibFopsGit))
            {
                System.IO.Directory.Delete(varLibFopsGit, true);
            }
        }
        
        /// <summary>
        /// 拉取最新代码
        /// </summary>
        public async Task<RunShellResult> PullAsync(int gitId, Action<string> actReceiveOutput)
        {
            var            info = await GitService.ToInfoAsync(gitId);
            RunShellResult runShellResult;

            // 获取Git存放的路径
            var gitPath = GetGitPath(info);
            
            // 判断git是否有clone过
            if (!System.IO.Directory.Exists(gitPath))
            {
                runShellResult = await CloneAsync(info, gitPath, actReceiveOutput);
            }
            else
            {
                runShellResult = await ShellTools.Run("git", $"-C {gitPath} pull --rebase", actReceiveOutput);
            }

            return runShellResult;
        }

        /// <summary>
        /// Clone代码
        /// </summary>
        private async Task<RunShellResult> CloneAsync(GitVO info, string path, Action<string> actReceiveOutput)
        {
            var url = info.Hub;
            // 需要密码
            if (!string.IsNullOrWhiteSpace(info.UserPwd))
            {
                url = url.Replace("//", $"//{info.UserName.Replace("@", "%40")}:{info.UserPwd}@");
            }

            return await ShellTools.Run("git", $"clone -b {info.Branch} {url} {path}", actReceiveOutput);
        }

        /// <summary>
        /// 获取Git存放的路径
        /// </summary>
        public string GetGitPath(GitVO info)
        {
            var gitName                           = info.Hub.Substring(info.Hub.LastIndexOf('/') + 1);
            if (gitName.EndsWith(".git")) gitName = gitName.Substring(0, gitName.Length - 4);
            return SavePath + gitName + "/";
        }
    }
}