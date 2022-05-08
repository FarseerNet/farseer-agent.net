using System.Net;
using System.Text;
using Docker.DotNet;
using Docker.DotNet.Models;
using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.Container.Entity;
using FarseerAgent.Domain.LogCollect.Container.Repository;

namespace FarseerAgent.Infrastructure.Repository;

public class ContainerApiRepository : IContainerApiRepository
{
    readonly DockerClient client = new DockerClientConfiguration().CreateClient();

    public async IAsyncEnumerable<ContainerDO> FindRunningContainerAsync()
    {
        // 显示当前正在运行的容器
        var listContainers = await client.Containers.ListContainersAsync(new ContainersListParameters());
        foreach (var container in listContainers)
        {
            // 元数据
            var inspectContainerAsync = await client.Containers.InspectContainerAsync(container.ID);

            // 容器IP
            inspectContainerAsync.NetworkSettings.Networks.TryGetValue("net", out var networks);
            // k8s的容器组名称
            inspectContainerAsync.Config.Labels.TryGetValue("io.kubernetes.container.name", out var k8sName);
            // 容器的启动入口名称(dotnet)
            if (inspectContainerAsync.Config.Entrypoint?.Count == 2 && inspectContainerAsync.Config.Entrypoint[0] == "dotnet")
            {
                k8sName = inspectContainerAsync.Config.Entrypoint[1];
                if (k8sName.EndsWith(".dll")) k8sName = k8sName.Substring(0, k8sName.Length - 4);
            }
            yield return new ContainerDO
            {
                Id    = container.ID,
                Name  = container.Names.FirstOrDefault().Substring(1),
                Image = inspectContainerAsync.Config.Image,
                Ip    = networks?.IPAddress,
                App = new ContainerAppVO
                {
                    Name = k8sName ?? container.Names.FirstOrDefault().Substring(1)
                },
                Env = inspectContainerAsync.Config.Env?.ToDictionary(o => o.Split('=')[0],
                                                                     o => o.Split('=')[1]),
            };
        }
    }

    public async Task<ContainerNodeVO> GetNodeInfoAsync()
    {
        var systemInfo = await client.System.GetSystemInfoAsync();
        //systemInfo.NCPU;
        //systemInfo.MemTotal;
        //systemInfo.Containers;容器数量
        //systemInfo.ContainersRunning;容器数量
        //systemInfo.ContainersPaused;容器数量
        //systemInfo.ContainersStopped;容器数量
        //systemInfo.Images;容器数量
        //host.docker.internal
        string hostIp = null;
        try
        {
            var addresses = await Dns.GetHostAddressesAsync("host.docker.internal");
            hostIp = addresses.Length > 0 ? addresses[0].ToString() : "";
        }
        catch
        {
        }

        return new ContainerNodeVO
        {
            Name = systemInfo.Name,
            Ip   = hostIp
        };
    }

    // /// <summary>
    // /// 读取日志
    // /// </summary>
    // public Task ReadLog(string id, Progress<string> progress)
    // {
    //     return client.Containers.GetContainerLogsAsync(id, new ContainerLogsParameters
    //     {
    //         ShowStdout = true,
    //         ShowStderr = false,
    //         Timestamps = true,
    //         Follow     = true,
    //     }, CancellationToken.None, progress);
    // }

    /// <summary>
    /// 读取日志
    /// </summary>
    public async Task ReadLog(string id, long lastReadLogTime, Progress<string> progress)
    {
        var logStream = await client.Containers.GetContainerLogsAsync(id, false, new ContainerLogsParameters
        {
            ShowStdout = true,
            ShowStderr = false,
            Timestamps = true,
            Follow     = true,
            Since      = lastReadLogTime.ToString()

        }, CancellationToken.None);

        await ReadOutputAsync(logStream, progress);
    }

    /// <summary>
    /// 读取日志流
    /// </summary>
    private async Task ReadOutputAsync(MultiplexedStream multiplexedStream, IProgress<string> progress, CancellationToken cancellationToken = default)
    {
        var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(81920);

        while (true)
        {
            Array.Clear(buffer, 0, buffer.Length);
            var readResult = await multiplexedStream.ReadOutputAsync(buffer, 0, buffer.Length, cancellationToken);

            if (readResult.EOF) break;
            if (readResult.Count > 0)
            {
                var responseLine = Encoding.UTF8.GetString(buffer, 0, readResult.Count);
                Console.WriteLine(responseLine);
                progress.Report(responseLine.Trim());
                Thread.Sleep(10);
            }
            else
            {
                break;
            }
        }

        System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
    }
}