using FarseerAgent.Domain.LogCollect.Container;
using FarseerAgent.Domain.LogCollect.Container.Entity;
using FS.Extends;
using FS.Mapper;

namespace FarseerAgent.Application.LogCollect.Entity;

/// <summary>
///     正在运行的容器
/// </summary>
[Map(typeof(ContainerDO))]
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

    public static implicit operator ContainerDO(ContainerDTO container)
    {
        var containerDo = container.Map<ContainerDO>();
        containerDo.App = new ContainerAppVO
        {
            Name = container.AppName
        };
        containerDo.Node = new ContainerNodeVO
        {
            Name = container.NodeName,
            Ip   = container.NodeIp
        };
        return containerDo;
    }

    /// <summary>
    /// 类型转换
    /// </summary>
    public static readonly Action<ContainerDTO, ContainerDO> Do2DtoRule = (dto, do2) =>
    {
        dto.AppName  = do2.App.Name;
        dto.NodeName = do2.Node.Name;
        dto.NodeIp   = do2.Node.Ip;
    };
}