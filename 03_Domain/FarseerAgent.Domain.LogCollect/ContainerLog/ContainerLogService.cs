using System.Globalization;
using System.Text.RegularExpressions;
using FarseerAgent.Domain.LogCollect.Container;
using FS.DI;
using FS.Utils.Common;
using Microsoft.Extensions.Logging;

namespace FarseerAgent.Domain.LogCollect.ContainerLog;

public class ContainerLogService : ISingletonDependency
{
    /// <summary>
    /// 将字符串日志，转换成对象
    /// </summary>
    public ContainerLogDO Analysis(ContainerDO container, string log)
    {
        //Console.WriteLine($"{container.Name}：{log}");
        // 解析日期时间
        var time = Regex.Match(log.Split(' ')[0].Replace("T", " "), "[\\d-:\\s]+").Value;
        DateTime.TryParseExact(time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var create);

        var logLevel = LogLevel.Information;
        var logLower = log.ToLower();

        if (logLower.Contains("error")) logLevel        = LogLevel.Error;
        else if (logLower.Contains("warning")) logLevel = LogLevel.Warning;
        else if (logLower.Contains("debug")) logLevel   = LogLevel.Debug;

        var logSpaceIndex = log.IndexOf(' ');

        return new ContainerLogDO
        {
            Id             = Encrypt.MD5(log),
            LogLevel       = logLevel,
            Content        = logSpaceIndex > 0 ? log.Substring(logSpaceIndex) : log,
            CreateAt       = create,
            AppName        = container.App.Name,
            ContainerEnv   = container.Env,
            ContainerImage = container.Image,
            ContainerIp    = container.Ip,
            ContainerName  = container.Name,
            NodeIp         = container.Node.Ip,
            NodeName       = container.Node.Name
        };
    }
}