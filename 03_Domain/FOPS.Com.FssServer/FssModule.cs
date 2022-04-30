using FS.Data;
using FS.Modules;

namespace FOPS.Com.FssServer
{
    /// <summary>
    ///     元信息模块
    /// </summary>
    [DependsOn(typeof(DataModule))]
    public class FssModule : FarseerModule
    {
        public override void PreInitialize()
        {
        }

        public override void PostInitialize()
        {
            IocManager.Container.Install(new FssInstaller(IocManager));
        }
    }
}