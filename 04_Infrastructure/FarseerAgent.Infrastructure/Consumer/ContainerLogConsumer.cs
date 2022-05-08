using FarseerAgent.Infrastructure.Repository.ContainerLog;
using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FS.DI;
using FS.MQ.Queue;
using FS.MQ.Queue.Attr;

namespace FarseerAgent.Infrastructure.Consumer
{
    /// <summary>
    ///     消费客户端
    /// </summary>
    [Consumer(Enable = true, Name = "ContainerLog", SleepTime = 1000)]
    public class ContainerLogConsumer : IListenerMessage
    {
        public async Task<bool> Consumer(List<object> queueList)
        {
            var lst = queueList.Select(o => (ContainerLogPO)o).ToList();
            foreach (var containerLogPO in lst.Where(o => !o.ContainerEnv.ContainsKey("farseer_logs")))
            {
                containerLogPO.ContainerEnv.TryAdd("farseer_logs", "app_log");
            }

            foreach (var containerLogPos in lst.GroupBy(o => o.ContainerEnv["farseer_logs"]))
            {
                await IocManager.GetService<ContainerLogAgent>().AddAsync(containerLogPos.Key, containerLogPos.ToList());
            }
            return true;
        }

        public Task<bool> FailureHandling(List<object> messages) => Task.FromResult(false);
    }
}