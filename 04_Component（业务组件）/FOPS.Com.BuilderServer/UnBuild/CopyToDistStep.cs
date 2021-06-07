using System;
using System.Threading.Tasks;
using FOPS.Abstract.Builder.Entity;
using FOPS.Abstract.Builder.Server;
using FOPS.Abstract.MetaInfo.Entity;
using FS.Core.Entity;
using FS.Utils.Common;

namespace FOPS.Com.BuilderServer.UnBuild
{
    public class CopyToDistStep:IBuildStep
    {
        const string SavePath = "/var/lib/fops/dist/";
        
        public Task<RunShellResult> Build(BuildEnvironment env, BuildVO build, ProjectVO project, GitVO git, Action<string> actReceiveOutput)
        {
            // 先删除之前编译的目标文件
            if (System.IO.Directory.Exists(env.ProjectReleaseDirRoot)) System.IO.Directory.Delete(env.ProjectReleaseDirRoot, true);
            System.IO.Directory.CreateDirectory(env.ProjectReleaseDirRoot);

            // 复制目录
            Files.CopyFolder(env.ProjectSourceDirRoot,env.ProjectReleaseDirRoot);

            return Task.FromResult(new RunShellResult(false,""));
        }
    }
}