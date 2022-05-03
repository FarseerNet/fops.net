using FOPS.Application.Build.Git.Entity;
using FOPS.Application.Build.Project.Entity;
using FOPS.Domain.Build.DeployK8S.Entity;
using FS.DI;

namespace FOPS.Abstract.Builder.Server
{
    public interface IDotnetOpr: ISingletonDependency
    {
        /// <summary>
        /// 获取项目的根目录
        /// </summary>
        string GetSourceDirRoot(BuildEnvironment buildEnvironment, ProjectDTO project, GitDTO git);

        /// <summary>
        /// 获取编译保存的目录地址
        /// </summary>
        string GetReleasePath(BuildEnvironment env, ProjectDTO project);
    }
}