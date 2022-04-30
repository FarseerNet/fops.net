using FS.Data;
using FS.Modules;

namespace FOPS.Com.DockerServer
{
    /// <summary>
    ///     元信息模块
    /// </summary>
    [DependsOn(typeof(DataModule))]
    public class DockerModule : FarseerModule
    {
        public override void PreInitialize()
        {
        }

        public override void PostInitialize()
        {
            IocManager.Container.Install(new DockerInstaller(IocManager));
        }
    }
}