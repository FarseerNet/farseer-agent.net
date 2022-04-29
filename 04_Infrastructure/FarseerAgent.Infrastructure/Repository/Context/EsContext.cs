using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FS.ElasticSearch;

namespace FarseerAgent.Infrastructure.Repository.Context;

/// <summary>
///     ES日志上下文
/// </summary>
public class EsContext : EsContext<EsContext>
{
    static EsContext()
    {
    }

    public EsContext() : base(configName: "log_es")
    {
    }

    /// <summary>
    ///     用户索引
    /// </summary>
    public IndexSet<ContainerLogPO> ContainerLogPO { get; set; }
}