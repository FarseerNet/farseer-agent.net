using FarseerAgent.Domain.LogCollect.ContainerLog;
using FS.Extends;
using FS.Mapper;
using Nest;

namespace FarseerAgent.Infrastructure.Repository.ContainerLog.Model;

[Map(typeof(ContainerLogDO)), ElasticsearchType(IdProperty = "Id")]
public class ContainerLogPO
{
    /// <summary>
    ///     主键
    /// </summary>
    [Keyword]
    public string Id { get; set; }

    /// <summary>
    ///     应用名称
    /// </summary>
    [Keyword]
    public string AppName { get; set; }

    /// <summary>
    ///     容器名称
    /// </summary>
    [Keyword]
    public string ContainerName { get; set; }

    /// <summary>
    ///     镜像名称
    /// </summary>
    [Keyword]
    public string ContainerImage { get; set; }

    /// <summary>
    ///     容器IP
    /// </summary>
    [Keyword]
    public string ContainerIp { get; set; }

    /// <summary>
    /// 环境变量
    /// </summary>
    [Flattened]
    public Dictionary<string, string> ContainerEnv { get; set; }

    /// <summary>
    ///     节点名称
    /// </summary>
    [Keyword]
    public string NodeName { get; set; }

    /// <summary>
    ///     节点IP
    /// </summary>
    [Keyword]
    public string NodeIp { get; set; }

    /// <summary>
    ///     日志级别
    /// </summary>
    [Keyword]
    public string LogLevel { get; set; }

    /// <summary>
    ///     日志内容
    /// </summary>
    [Text]
    public string Content { get; set; }

    /// <summary>
    ///     日志时间
    /// </summary>
    [Date]
    public DateTime CreateAt { get; set; }

    public static implicit operator ContainerLogPO(ContainerLogDO log)
    {
        var po = log.Map<ContainerLogPO>();
        po.LogLevel = log.LogLevel.ToString();
        return po;
    }
}