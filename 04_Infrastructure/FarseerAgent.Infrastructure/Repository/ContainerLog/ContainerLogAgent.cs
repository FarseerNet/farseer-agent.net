using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FarseerAgent.Infrastructure.Repository.Context;
using FS.DI;

namespace FarseerAgent.Infrastructure.Repository.ContainerLog;

public class ContainerLogAgent : ISingletonDependency
{
    /// <summary>
    ///     写入ES或数据库
    /// </summary>
    public Task AddAsync(string indexName, IEnumerable<ContainerLogPO> lstLog)
    {
        return EsContext.Data.ContainerLogPO.SetName(indexName).InsertAsync(lstLog);
    }
}