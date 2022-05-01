using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public class GitPullAllStep : IBuildStep
    {
        public IGitOpr          GitOpr          { get; set; }
        public IGitService      GitService      { get; set; }
        public IBuildLogService BuildLogService { get; set; }

        /// <summary>
        /// 拉取全部
        /// </summary>
        public async Task<RunShellResult> Build(BuildEnvironment env, BuildDTO build, ProjectDTO project, GitDTO git, Action<string> actReceiveOutput, CancellationToken cancellationToken)
        {
            var lstGitIds = new List<int>
            {
                project.GitId
            };
            lstGitIds.AddRange(project.DependentGitIds);
            lstGitIds.Remove(0);
            
            var lstGit = await GitService.ToListAsync(lstGitIds);
            foreach (var gitVO in lstGit)
            {
                BuildLogService.Write(build.Id, "---------------------------------------------------------");
                BuildLogService.Write(build.Id, $"开始拉取git {gitVO.Name} 分支：{gitVO.Branch} 仓库：{gitVO.Hub}。");

                // 获取当前git的存储路径
                var gitDirRoot = GitOpr.GetGitPath(env, gitVO);
                // 根据判断是否存在Git目录，来决定返回Clone or pull
                var buildStep = GitOpr.GetGitStep(gitDirRoot);
                var result    = await buildStep.Build(env, build, project, gitVO, actReceiveOutput, cancellationToken);
                if (result.IsError)
                {
                    return new RunShellResult(true, $"拉取出错了。");
                }
            }
            BuildLogService.Write(build.Id, $"检查源文件。");
            if (!System.IO.Directory.Exists(env.ProjectSourceDirRoot))
            {
                return new RunShellResult(true, $"源文件：{env.ProjectSourceDirRoot} 不存在，请检查项目设置。");
            }
            return new RunShellResult(false, $"拉取完成。");
        }
    }
}