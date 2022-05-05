using FOPS.Domain.AppLog.ContainerLog;
using FOPS.Infrastructure.Repository.ContainerLog.Model;
using FS.Cache.Redis;
using FS.Core;
using FS.Data;
using FS.ElasticSearch;
using FS.Extends;
using FS.Mapper;
using FS.Modules;
using FS.MQ.RedisStream;
using Mapster;

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


    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(type: GetType());
        
        TypeAdapterConfig<ContainerLogDO, ContainerLogPO>.NewConfig().Unflattening(true)
                                                         .Map(dest => dest.CreateAt,
                                                              src => src.CreateAt.ToTimestamps());
        
        TypeAdapterConfig<ContainerLogPO, ContainerLogDO>.NewConfig().Unflattening(true)
                                                         .Map(dest => dest.CreateAt,
                                                              src => src.CreateAt.ToTimestamps());
    }
}