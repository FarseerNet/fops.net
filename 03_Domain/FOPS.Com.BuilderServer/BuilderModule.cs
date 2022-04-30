using FS.Data;
using FS.Modules;

namespace FOPS.Com.BuilderServer
{
    /// <summary>
    ///     模块
    /// </summary>
    [DependsOn(typeof(DataModule))]
    public class BuilderModule : FarseerModule
    {
        public override void PreInitialize()
        {
        }

        public override void PostInitialize()
        {
            IocManager.Container.Install(new BuilderInstaller(IocManager));
        }
    }
}