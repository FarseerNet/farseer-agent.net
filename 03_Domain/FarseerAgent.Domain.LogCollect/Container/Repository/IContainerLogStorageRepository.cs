using FarseerAgent.Domain.LogCollect.ContainerLog;
using FS.DI;

namespace FarseerAgent.Domain.LogCollect.Container.Repository;

/// <summary>
/// 容器日志的存储
/// </summary>
public interface IContainerLogStorageRepository : ISingletonDependency
{
    /// <summary>
    ///     将日志写入队列中
    /// </summary>
    void Add(ContainerLogDO log);
    /// <summary>
    /// 检查日志ID是否已采集过
    /// </summary>
    bool ExistsLogId(string containerId, string logId);
}