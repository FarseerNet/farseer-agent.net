using System.Collections.Concurrent;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FarseerAgent.Domain.LogCollect.ContainerLog;
using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FS.Core.Abstract.MQ.Queue;
using FS.DI;
using FS.Extends;

namespace FarseerAgent.Infrastructure.Repository;

public class ContainerLogStorageRepository : IContainerLogStorageRepository
{
    readonly IQueueProduct _queueProduct;
    public ContainerLogStorageRepository()
    {
        _queueProduct = IocManager.GetService<IQueueProduct>(name: "ContainerLog");
    }

    /// <summary>
    /// 代表已采集过的日志，当容器重启后，不需要再次采集
    /// </summary>
    private ConcurrentDictionary<string, long> ContainerLogId { get; set; } = new();

    /// <summary>
    /// 读取容器日志的最后读取时间
    /// </summary>
    public long GetLastReadLogTime(string containerId)
    {
        if (!ContainerLogId.ContainsKey(containerId)) ContainerLogId.TryAdd(containerId, 0);
        return ContainerLogId[containerId];
    }
    
    /// <summary>
    ///     将日志写入队列中
    /// </summary>
    public void Add(ContainerLogDO log)
    {
        ContainerLogId[log.ContainerId] = log.CreateAt.ToTimestamps();
        _queueProduct.Send((ContainerLogPO)log);
    }
}