using FarseerAgent.Application.LogCollect.Entity;
using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FarseerAgent.Domain.LogCollect.ContainerLog;
using FS.DI;
using FS.Extends;

namespace FarseerAgent.Application.LogCollect;

public class LogCollectApp : ISingletonDependency
{
    public ContainerService               ContainerService              { get; set; }
    public ContainerLogService            ContainerLogService           { get; set; }
    public IContainerLogStorageRepository ContainerLogStorageRepository { get; set; }

    /// <summary>
    ///    查找未采集日志的容器
    /// </summary>
    public Task<List<ContainerDTO>> FindNewContainerAsync() => ContainerService.FindNewContainerAsync().MapAsync<ContainerDTO, ContainerDO>(ContainerDTO.Do2DtoRule);

    /// <summary>
    /// 读取容器中的日志
    /// </summary>
    public Task ReadLog(ContainerDTO container)
    {
        return ContainerService.ReadLog(container.Id, container.Name, new Progress<string>(o =>
        {
            // 解析为本地的日志对象
            var log = ContainerLogService.Analysis(container, o);
            ContainerLogStorageRepository.Add(log);
        }));
    }
}