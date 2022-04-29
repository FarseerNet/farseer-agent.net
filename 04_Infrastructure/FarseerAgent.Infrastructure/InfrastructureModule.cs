using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.ContainerLog;
using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FarseerAgent.Infrastructure.Repository.Queue;
using FS.ElasticSearch;
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

        var cts               = new CancellationTokenSource();
        var containerLogQueue = new ContainerLogQueue();
        containerLogQueue.StartDequeue(cancellationToken: cts.Token);
        IocManager.Register(containerLogQueue);
        
        TypeAdapterConfig<ContainerLogDO, ContainerLogPO>.NewConfig().Unflattening(true);
    }
}