using FS.Data;
using FS.Modules;

namespace FOPS.Com.K8SServer
{
    /// <summary>
    ///     元信息模块
    /// </summary>
    [DependsOn(typeof(DataModule))]
    public class K8SModule : FarseerModule
    {
        public override void PreInitialize()
        {
        }

        public override void PostInitialize()
        {
            IocManager.Container.Install(new K8SInstaller(IocManager));
        }
    }
}