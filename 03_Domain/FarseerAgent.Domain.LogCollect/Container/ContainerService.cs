using FarseerAgent.Domain.LogCollect.Container.Entity;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FS.DI;
using Microsoft.Extensions.Logging;

namespace FarseerAgent.Domain.LogCollect.Container;

public class ContainerService : ISingletonDependency
{
    public        IContainerApiRepository ContainerApiRepository { get; set; }
    public static List<string>            MonitorContainer       { get; } = new();
    public static ContainerNodeVO         ContainerNode          { get; set; }

    /// <summary>
    ///    查找正在运行的容器
    /// </summary>
    public async IAsyncEnumerable<ContainerDO> FindRunningContainerAsync()
    {
        var containers = ContainerApiRepository.FindRunningContainerAsync();
        await foreach (var containerDO in containers)
        {
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

    /// <summary>
    /// 读取容器中的日志
    /// </summary>
    public async Task ReadLog(string containerId, string containerName, Progress<string> progress)
    {
        IocManager.Instance.Logger<ContainerService>().LogInformation($"发现新的容器：{containerName} {containerId}，开始监控...");

        try
        {
            MonitorContainer.Add(containerId);
            // 调用仓储层的容器接口
            await ContainerApiRepository.ReadLog(containerId, progress);
        }
        catch (Exception e)
        {
            IocManager.Instance.Logger<ContainerService>().LogError(e.Message);
        }
        finally
        {
            // 容器停止后，移除监控列表
            MonitorContainer.Remove(containerId);
            IocManager.Instance.Logger<ContainerService>().LogInformation($"容器：{containerName} {containerId}，已停止，移除监控...");
        }
    }
}