using FOPS.Domain.AppLog;
using FOPS.Domain.Build;
using FOPS.Domain.LinkTrack;
using FOPS.Domain.Sys;
using FS.Modules;

namespace FOPS.Application;

[DependsOn(
              typeof(AppLogModule),
              typeof(BuildModule),
              typeof(LinkTrackModule),
              typeof(SysModule))]
public class ApplicationModule : FarseerModule
{
    /// <summary>
    ///     初始化之前
    /// </summary>
    public override void PreInitialize()
    {
    }

    public override void Initialize()
    {
        //IocManager.RegisterAssemblyByConvention(type: GetType());
    }
}