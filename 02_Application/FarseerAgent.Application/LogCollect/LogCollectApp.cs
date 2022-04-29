using FarseerAgent.Application.LogCollect.Entity;
using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FarseerAgent.Domain.LogCollect.ContainerLog;
using FS.DI;
using FS.Extends;
using Mapster;

namespace FarseerAgent.Application.LogCollect;

public class LogCollectApp : ISingletonDependency
{
    public ContainerService               ContainerService              { get; set; }
    public ContainerLogService            ContainerLogService           { get; set; }
    public IContainerLogStorageRepository ContainerLogStorageRepository { get; set; }

    /// <summary>
    ///    查找未采集日志的容器
    /// </summary>
    public async IAsyncEnumerable<ContainerDTO> FindNewContainerAsync()
    {
        //return ContainerService.FindNewContainerAsync().Adapt<Task<List<ContainerDTO>>>();
        await foreach (var containerDO in ContainerService.FindNewContainerAsync())
        {
            yield return containerDO.Adapt<ContainerDTO>();
        }
    }

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
            Thread.Sleep(10); // 休眠一下
        }));
    }
}