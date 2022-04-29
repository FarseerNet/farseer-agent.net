using FarseerAgent.Domain.LogCollect;
using FS.Modules;

namespace FarseerAgent.Application;

[DependsOn(typeof(LogCollectModule))]
public class ApplicationModule : FarseerModule
{
    /// <summary>
    ///     初始化之前
    /// </summary>
    public override void PreInitialize()
    {
    }

    /// <summary>
    ///     初始化
    /// </summary>
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(type: GetType());
    }
}