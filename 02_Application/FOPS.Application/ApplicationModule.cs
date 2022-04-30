using FOPS.Com.BuilderServer;
using FOPS.Com.DockerServer;
using FOPS.Com.FssServer;
using FOPS.Com.K8SServer;
using FOPS.Com.MetaInfoServer;
using FS.Modules;

namespace FOPS.Application;

[DependsOn(
              typeof(FssModule),
              typeof(K8SModule),
              typeof(BuilderModule),
              typeof(DockerModule),
              typeof(MetaInfoModule))]
public class ApplicationModule : FarseerModule
{
    /// <summary>
    ///     初始化之前
    /// </summary>
    public override void PreInitialize()
    {
    }

    /// <summary>
    ///     初始化
    /// </summary>
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(type: GetType());
    }
}