using FS.Cache.Redis;
using FS.Core;
using FS.Data;
using FS.ElasticSearch;
using FS.Mapper;
using FS.Modules;
using FS.MQ.RedisStream;

namespace FOPS.Infrastructure;

[DependsOn(typeof(MapperModule),
              typeof(FarseerCoreModule),
              typeof(RedisModule),
              typeof(RedisStreamModule),
              typeof(DataModule),
              typeof(ElasticSearchModule))]
public class InfrastructureModule : FarseerModule
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