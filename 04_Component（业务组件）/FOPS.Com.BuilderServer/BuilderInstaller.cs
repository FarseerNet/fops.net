using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FOPS.Abstract.Builder.Server;
using FOPS.Com.BuilderServer.Docker;
using FOPS.Com.BuilderServer.Dotnet;
using FOPS.Com.BuilderServer.Git;
using FOPS.Com.BuilderServer.Kubectl;
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
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            container.Register(Component.For<IBuildStep, DockerBuildStep>().LifestyleTransient());
            container.Register(Component.For<IBuildStep, DockerLoginStep>().LifestyleTransient());
            container.Register(Component.For<IBuildStep, DockerUploadStep>().LifestyleTransient());
            container.Register(Component.For<IBuildStep, DotnetBuildStep>().LifestyleTransient());
            container.Register(Component.For<IBuildStep, GitCloneStep>().LifestyleTransient());
            container.Register(Component.For<IBuildStep, GitPullAllStep>().LifestyleTransient());
            container.Register(Component.For<IBuildStep, GitPullStep>().LifestyleTransient());
            container.Register(Component.For<IBuildStep, KubectlUpdateImageStep>().LifestyleTransient());
        }
    }
}