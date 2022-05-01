using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FOPS.Abstract.Builder.Server;
using FOPS.Com.BuilderServer.Build;
using FOPS.Com.BuilderServer.Docker;
using FOPS.Com.BuilderServer.Dotnet;
using FOPS.Com.BuilderServer.Git;
using FOPS.Com.BuilderServer.Kubectl;
using FOPS.Com.BuilderServer.Shell;
using FOPS.Com.BuilderServer.UnBuild;
using FS.DI;

namespace FOPS.Com.BuilderServer
{
    public class BuilderInstaller : IWindsorInstaller
    {
        /// <summary>
        ///     依赖获取接口
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        ///     构造函数
        /// </summary>
        public BuilderInstaller(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        /// <summary>
        ///     注册依赖
        /// </summary>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBuildStep, DockerBuildStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, DockerLoginStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, DockerUploadStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, DotnetBuildStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, GitCloneStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, GitPullAllStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, GitPullStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, KubectlUpdateImageStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, ShellStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, CopyToDistStep>().LifestyleSingleton());
            container.Register(Component.For<IBuildStep, CheckStep>().LifestyleSingleton());
        }
    }
}