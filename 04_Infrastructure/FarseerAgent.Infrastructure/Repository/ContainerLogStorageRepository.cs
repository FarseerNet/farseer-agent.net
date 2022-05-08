using System.Collections.Concurrent;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FarseerAgent.Domain.LogCollect.ContainerLog;
using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FS.DI;
using FS.MQ.Queue;

namespace FarseerAgent.Infrastructure.Repository;

public class ContainerLogStorageRepository : IContainerLogStorageRepository
{
    readonly IQueueProduct _queueProduct;
    public ContainerLogStorageRepository()
    {
        _queueProduct = IocManager.GetService<IQueueManager>(name: "ContainerLog").Product;
    }

    /// <summary>
    /// 代表已采集过的日志，当容器重启后，不需要再次采集
    /// </summary>
    private static ConcurrentDictionary<string, List<string>> ContainerLogId { get; set; } = new();

    /// <summary>
    ///     将日志写入队列中
    /// </summary>
    public void Add(ContainerLogDO log)
    {
        _queueProduct.Send((ContainerLogPO)log);
        ContainerLogId[log.ContainerId].Add(log.Id);
    }

    /// <summary>
    /// 检查日志ID是否已采集过
    /// </summary>
    public bool ExistsLogId(string containerId, string logId)
    {
        if (!ContainerLogId.ContainsKey(containerId))
        {
            ContainerLogId.TryAdd(containerId, new List<string>());
            return false;
        }

        // 如果日志已采集过，则不用再放入队列
        return ContainerLogId[containerId].Contains(logId);
    }
}