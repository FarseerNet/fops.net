using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FOPS.Abstract.MetaInfo.Server;
using FS.Core.Entity;

namespace FOPS.Com.BuilderServer.Git
{
    /// <summary>
    /// 拉取全部Git库
    /// </summary>
    public class GitPullAllStep:IBuildStep
    {
        public IGitOpr          GitOpr          { get; set; }
        public IGitService      GitService      { get; set; }
        public IBuildLogService BuildLogService { get; set; }
        
        /// <summary>
        /// 拉取全部
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            var lstGit = await GitService.ToListAsync();
            foreach (var gitVO in lstGit)
            {
                BuildLogService.Write(build.Id, "---------------------------------------------------------");
                BuildLogService.Write(build.Id, $"开始拉取git {gitVO.Name} 分支：{gitVO.Branch} 仓库：{gitVO.Hub}。");
                
                // 获取当前git的存储路径
                var gitDirRoot = GitOpr.GetGitPath(gitVO);
                // 根据判断是否存在Git目录，来决定返回Clone or pull
                var buildStep  =  GitOpr.GetGitStep(gitDirRoot);
                var result     = await buildStep.Build(env, build, project, gitVO, actReceiveOutput);
                if (result.IsError)
                {
                    BuildLogService.Write(build.Id, $"拉取出错了。");
                    return result;
                }
            }

            BuildLogService.Write(build.Id, $"拉取完成。");
            // 拉取全部成功
            return new RunShellResult(false, "");
        }
    }
}