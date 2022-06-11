using FarseerAgent.Domain.LogCollect.Container.Repository;
using FS.Modules;

namespace FarseerAgent.Domain.LogCollect;

public class LogCollectModule : FarseerModule
{
    /// <summary>
    ///     初始化
    /// </summary>
    public override void Initialize()
    {
    }

    public override void PostInitialize()
    {
        try
        {
            // 获取容器的主机信息
            var nodeInfoTask = FS.DI.IocManager.GetService<IContainerApiRepository>().GetNodeInfoAsync();
            Task.WhenAll(nodeInfoTask);
            ContainerFindService.ContainerNode = nodeInfoTask.Result;
        }
        catch (Exception e)
        {
            if (e.InnerException is
                {
                    Message: "Connection failed"
                })
            {
                throw new Exception("容器连接失败。请检查容器daemon是否在运行。");
            }
            throw;
        }
    }
}