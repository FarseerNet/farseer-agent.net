using FarseerAgent.Domain.LogCollect.Container.Entity;
using FS.DI;

namespace FarseerAgent.Domain.LogCollect.Container.Repository;

/// <summary>
/// 容器接口操作
/// </summary>
public interface IContainerApiRepository : ISingletonDependency
{
    /// <summary>
    ///    查找正在运行的容器
    /// </summary>
    public IAsyncEnumerable<ContainerDO> FindRunningContainerAsync();
    /// <summary>
    /// 读取日志
    /// </summary>
    Task ReadLog(string id, Progress<string> progress);
    /// <summary>
    /// 获取容器宿主的信息
    /// </summary>
    Task<ContainerNodeVO> GetNodeInfoAsync();
}