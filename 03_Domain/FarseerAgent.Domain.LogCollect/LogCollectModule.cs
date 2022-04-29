using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FS.Modules;

namespace FarseerAgent.Domain.LogCollect;

public class LogCollectModule : FarseerModule
{
    /// <summary>
    ///     初始化
    /// </summary>
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(type: GetType());
    }

    public override void PostInitialize()
    {
        // 获取容器的主机信息
        var nodeInfoTask = FS.DI.IocManager.GetService<IContainerApiRepository>().GetNodeInfoAsync();
        Task.WhenAll(nodeInfoTask);
        ContainerService.ContainerNode = nodeInfoTask.Result;
    }
}