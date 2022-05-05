using FarseerAgent.Infrastructure.Repository.ContainerLog.Model;
using FarseerAgent.Infrastructure.Repository.Context;
using FS.DI;

namespace FarseerAgent.Infrastructure.Repository.ContainerLog;

public class ContainerLogAgent : ISingletonDependency
{
    /// <summary>
    ///     写入ES或数据库
    /// </summary>
    public async Task<int> AddAsync(string indexName, List<ContainerLogPO> lstLog)
    {
        return 0;
        await EsContext.Data.ContainerLogPO.SetName(indexName).InsertAsync(lst: lstLog);
        return lstLog.Count;
    }
}