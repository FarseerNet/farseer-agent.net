using FarseerAgent.Domain.LogCollect.Container.Repository;
using FarseerAgent.Domain.LogCollect.ContainerLog;
using FarseerAgent.Infrastructure.Repository.Queue;

namespace FarseerAgent.Infrastructure.Repository;

public class ContainerLogStorageRepository : IContainerLogStorageRepository
{
    public ContainerLogQueue ContainerLogQueue { get; set; }

    /// <summary>
    /// 代表已采集过的日志，当容器重启后，不需要再次采集
    /// </summary>
    private static Dictionary<string, List<string>> ContainerLogId { get; set; } = new();

    /// <summary>
    ///     将日志写入队列中
    /// </summary>
    public void Add(ContainerLogDO log)
    {
        var key = $"{log.AppName}_{log.ContainerIp}";
        if (!ContainerLogId.ContainsKey(key)) ContainerLogId.TryAdd(key, new List<string>());
        // 如果日志已采集过，则不用再放入队列
        if (ContainerLogId[key].Contains(log.Id)) return;
        ContainerLogQueue.Enqueue(log);
        ContainerLogId[key].Add(log.Id);
    }
}