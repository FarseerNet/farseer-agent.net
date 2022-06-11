using FarseerAgent.Domain.LogCollect.Container;
using FS.Extends;

namespace FarseerAgent.Application.LogCollect.Entity;

/// <summary>
///     正在运行的容器
/// </summary>
public class ContainerDTO
{
    /// <summary>
    /// 实例ID
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 实例名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     容器IP
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    ///     镜像名称
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    public string NodeName { get; set; }

    /// <summary>
    /// 节点IP
    /// </summary>
    public string NodeIp { get; set; }

    /// <summary>
    /// 容器中的环境变量
    /// </summary>
    public Dictionary<string, string> Env { get; set; }

    public static implicit operator ContainerDO(ContainerDTO container) => container.Map<ContainerDO>();
}