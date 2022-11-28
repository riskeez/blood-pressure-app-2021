using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BPLog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((context, config) =>
                {
                    config.ClearProviders();

                    var logConfig = new LoggerConfiguration();

#if (DEBUG)
                    logConfig.MinimumLevel.Debug();
#else
                    logConfig.MinimumLevel.Warning();
#endif

                    logConfig.WriteTo.File(@"logs.txt", rollOnFileSizeLimit: true);
                    
                    config.AddSerilog(logConfig.CreateLogger());
                });
    }
}
