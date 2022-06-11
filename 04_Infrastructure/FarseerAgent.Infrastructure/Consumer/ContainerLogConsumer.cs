using Collections.Pooled;
using FarseerAgent.Infrastructure.Repository.ContainerLog;
using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FS.Core.Abstract.MQ.Queue;
using FS.DI;
using FS.MQ.Queue.Attr;

namespace FarseerAgent.Infrastructure.Consumer
{
    /// <summary>
    ///     消费客户端
    /// </summary>
    [Consumer(Enable = true, Name = "ContainerLog", SleepTime = 1000)]
    public class ContainerLogConsumer : IListenerMessage
    {
        public ContainerLogAgent ContainerLogAgent { get; set; }

        public async Task<bool> Consumer(IEnumerable<object> queueList)
        {
            using var lst = queueList.Select(o => (ContainerLogPO)o).ToPooledList();
            foreach (var containerLogPO in lst.Where(o => !o.ContainerEnv.ContainsKey("farseer_logs")))
            {
                containerLogPO.ContainerEnv.TryAdd("farseer_logs", "app_log");
            }

            foreach (var containerLogPos in lst.GroupBy(o => o.ContainerEnv["farseer_logs"]))
            {
                await ContainerLogAgent.AddAsync(containerLogPos.Key, containerLogPos);
            }
            return true;
        }

        public Task<bool> FailureHandling(IEnumerable<object> messages) => Task.FromResult(false);
    }
}