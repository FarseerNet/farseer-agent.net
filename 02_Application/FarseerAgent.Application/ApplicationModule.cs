using FarseerAgent.Application.LogCollect.Entity;
using FarseerAgent.Domain.LogCollect;
using FarseerAgent.Domain.LogCollect.Container;
using FS.Modules;
using Mapster;

namespace FarseerAgent.Application;

[DependsOn(typeof(LogCollectModule))]
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
        TypeAdapterConfig<ContainerDTO, ContainerDO>.NewConfig().Unflattening(true);
    }
}