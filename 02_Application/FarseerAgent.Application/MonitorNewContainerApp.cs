using FarseerAgent.Domain.LogCollect;
using FarseerAgent.Domain.LogCollect.Container.Repository;
using FS.DI;
using FS.Utils.Common;

namespace FarseerAgent.Application;

public class MonitorNewContainerApp : ISingletonDependency
{
    public ContainerFindService           ContainerFindService          { get; set; }
    public ContainerReadLogService        ContainerReadLogService       { get; set; }
    public ContainerLogAnalysisService    ContainerLogAnalysisService   { get; set; }
    public IContainerLogStorageRepository ContainerLogStorageRepository { get; set; }

    /// <summary>
    ///    查找未采集日志的容器，并读取容器中的日志
    /// </summary>
    public async Task MonitorAsync()
    {
        await foreach (var containerDO in ContainerFindService.FindNewContainerAsync())
        {
            ContainerReadLogService.Read(containerDO.Id, containerDO.Name, new Progress<string>(o =>
            {
                var logId = Encrypt.MD5(o);
                if (ContainerLogStorageRepository.ExistsLogId(containerDO.Id, logId)) return;

                // 解析为本地的日志对象
                var log = ContainerLogAnalysisService.Analysis(containerDO, o);
                log.Id = logId;
                
                ContainerLogStorageRepository.Add(log);
            }));
        }
    }
}