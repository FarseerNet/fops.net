using FOPS.Com.BuilderServer;
using FOPS.Com.DockerServer;
using FOPS.Com.FssServer;
using FOPS.Com.K8SServer;
using FOPS.Com.MetaInfoServer;
using FOPS.Domain.AppLog;
using FOPS.Domain.Build;
using FOPS.Domain.LinkTrack;
using FOPS.Domain.Fss;
using FOPS.Domain.Sys;
using FS.Modules;

namespace FOPS.Application;

[DependsOn(
              typeof(AppLogModule),
              typeof(BuildModule),
              typeof(Fss2Module),
              typeof(LinkTrackModule),
              typeof(SysModule),
              
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