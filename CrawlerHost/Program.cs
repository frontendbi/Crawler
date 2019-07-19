using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CrawlerHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    if (File.Exists("appsettings.json"))
                    {
                        config.AddJsonFile("appsettings.json");
                    }
                    config.AddEnvironmentVariables("CRAWLER_HOST_");
                })
                .ConfigureLogging(config =>
                {
                    config.AddSerilog();
                    var configure = new LoggerConfiguration()
#if DEBUG
                        .MinimumLevel.Verbose()
#else
						.MinimumLevel.Information()
#endif
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                    Log.Logger = configure.CreateLogger();
                })
                .Build();


            await host.RunAsync();
        }
    }
}
