using FarseerAgent.Application.LogCollect;
using FS.Core.Job;
using FS.Tasks;

namespace FarseerAgent.Service.Job;

/// <summary>
/// 发现新的容器
/// </summary>
[Job(Interval = 10000)]
public class FindNewContainerJob : IJob
{
    public LogCollectApp LogCollectApp { get; set; }

    public async Task Execute(ITaskContext context)
    {
        // 找到新的容器
        var listContainers = await LogCollectApp.FindNewContainerAsync();

        // 解析并存储日志
        foreach (var container in listContainers)
        {
            LogCollectApp.ReadLog(container);
        }
    }
}