using FarseerAgent.Domain.LogCollect.Container.Repository;
using FS.DI;
using Microsoft.Extensions.Logging;

namespace FarseerAgent.Domain.LogCollect;

public class ContainerReadLogService : ISingletonDependency
{
    public IContainerApiRepository ContainerApiRepository { get; set; }

    /// <summary>
    /// 读取容器中的日志
    /// </summary>
    public async Task Read(string containerId, string containerName, Progress<string> progress)
    {
        IocManager.Instance.Logger<ContainerReadLogService>().LogInformation($"发现新的容器：{containerName} {containerId}，开始监控...");

        try
        {
            ContainerFindService.MonitorContainer.Add(containerId);
            // 调用仓储层的容器接口
            await ContainerApiRepository.ReadLog(containerId, progress);
        }
        catch (Exception e)
        {
            IocManager.Instance.Logger<ContainerFindService>().LogError(e.Message);
        }
        finally
        {
            // 容器停止后，移除监控列表
            ContainerFindService.MonitorContainer.Remove(containerId);
            IocManager.Instance.Logger<ContainerFindService>().LogInformation($"容器：{containerName} {containerId}，已停止，移除监控...");
        }
    }
}