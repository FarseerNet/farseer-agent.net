using FarseerAgent.Infrastructure.Repository.Queue;
using FS.ElasticSearch;
using FS.EventBus;
using FS.Mapper;
using FS.Modules;
using FS.Tasks;

namespace FarseerAgent.Infrastructure;

[DependsOn(typeof(MapperModule), typeof(ElasticSearchModule), typeof(EventBusModule), typeof(TaskModule))]
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
    }
}