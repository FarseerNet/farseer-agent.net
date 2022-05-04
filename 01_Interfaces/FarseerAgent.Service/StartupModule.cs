using FarseerAgent.Application;
using FarseerAgent.Infrastructure;
using FS.Modules;

namespace FarseerAgent.Service
{
    /// <summary>
    ///     启动模块
    /// </summary>
    [DependsOn(typeof(InfrastructureModule), typeof(ApplicationModule))] // 依赖Job模块
    public class StartupModule : FarseerModule
    {
    }
}