using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.Container.Entity;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FS.DI;
using Microsoft.Extensions.Logging;

namespace FarseerAgent.Domain.LogCollect;

/// <summary>
/// 容器查找服务
/// </summary>
public class ContainerFindService : ISingletonDependency
{
    public        IContainerApiRepository ContainerApiRepository { get; set; }
    public static ContainerNodeVO         ContainerNode          { get; set; }
    public static List<string>            MonitorContainer       { get; } = new();

    /// <summary>
    ///    查找正在运行的容器
    /// </summary>
    public async IAsyncEnumerable<ContainerDO> FindRunningContainerAsync()
    {
        var containers = ContainerApiRepository.FindRunningContainerAsync();
        await foreach (var containerDO in containers)
        {
            if (containerDO.App.Name == "FarseerAgent.Service") continue;
            //if (!containerDO.Env.ContainsKey("farseer_logs"))
            containerDO.Node = ContainerNode;
            yield return containerDO;
        }
    }

    /// <summary>
    ///    查找未采集日志的容器，并加入到监控中
    /// </summary>
    public async IAsyncEnumerable<ContainerDO> FindNewContainerAsync()
    {
        await foreach (var container in FindRunningContainerAsync())
        {
            if (MonitorContainer.Contains(container.Id)) continue;
            yield return container;
        }
    }
}