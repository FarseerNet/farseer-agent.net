using FarseerAgent.Domain.LogCollect.Container.Entity;

namespace FarseerAgent.Domain.LogCollect.Container;

/// <summary>
///     正在运行的容器
/// </summary>
public class ContainerDO
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
    /// 容器中的应用
    /// </summary>
    public ContainerAppVO App { get; set; }

    /// <summary>
    ///     节点
    /// </summary>
    public ContainerNodeVO Node { get; set; }
    
    /// <summary>
    /// 容器中的环境变量
    /// </summary>
    public Dictionary<string,string> Env { get; set; }
}