using FarseerAgent.Infrastructure.Repository.ContainerLog;
using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FS.Core.Async;
using FS.DI;

namespace FarseerAgent.Infrastructure.Repository.Queue
{
    /// <summary>
    /// 日志队列
    /// </summary>
    public class ContainerLogQueue : BaseAsyncQueue<ContainerLogPO>, ISingletonDependency
    {
        public ContainerLogQueue() : base(maxQueueSize: 500000, callBackListCapacity: 1000, sleepMs: 500)
        {
        }

        /// <summary>
        /// 将队列中的日志写入ES
        /// </summary>
        protected override async Task OnDequeue(List<ContainerLogPO> lst, int remainCount)
        {
            foreach (var containerLogPO in lst.Where(o => !o.ContainerEnv.ContainsKey("farseer_logs")))
            {
                containerLogPO.ContainerEnv.TryAdd("farseer_logs", "app_log");
            }
            foreach (var containerLogPos in lst.GroupBy(o => o.ContainerEnv["farseer_logs"]))
            {
                await IocManager.GetService<ContainerLogAgent>().AddAsync(containerLogPos.Key, containerLogPos.ToList());
            }
        }
    }
}