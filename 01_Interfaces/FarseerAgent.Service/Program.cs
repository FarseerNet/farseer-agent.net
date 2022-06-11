using FS;
using FS.Tasks;

namespace FarseerAgent.Service
{
    [Tasks] // 开启后，才能把JOB自动注册进来
    public class Program
    {
        public static void Main() // 485ms，内存占用76M
        {
            // 初始化模块
            FarseerApplication.Run<StartupModule>().Initialize();
            Thread.Sleep(millisecondsTimeout: -1);
        }
    }
}