using FarseerAgent.Application.LogCollect.Entity;
using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.ContainerLog;
using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FS.ElasticSearch;
using FS.Extends;
using FS.Modules;
using FS.Tasks;
using Mapster;

namespace FarseerAgent.Infrastructure;

[DependsOn(typeof(ElasticSearchModule), typeof(TaskModule))]
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

        TypeAdapterConfig<ContainerDTO, ContainerDO>.NewConfig().Unflattening(true);
        TypeAdapterConfig<ContainerLogDO, ContainerLogPO>.NewConfig().Unflattening(true)
                                                         .Map(dest => dest.CreateAt,
                                                              src => src.CreateAt.ToTimestamps());
        
        TypeAdapterConfig<ContainerLogPO, ContainerLogDO>.NewConfig().Unflattening(true)
                                                         .Map(dest => dest.CreateAt,
                                                              src => src.CreateAt.ToTimestamps());
    }
}