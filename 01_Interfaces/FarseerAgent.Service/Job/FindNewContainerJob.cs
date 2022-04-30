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
        // 解析并存储日志
        await foreach (var container in LogCollectApp.FindNewContainerAsync())
        {
            Console.WriteLine($"foreach:{container.Id}");
            LogCollectApp.ReadLog(container);
        }
    }
}