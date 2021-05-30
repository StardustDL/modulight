using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.CommandLine;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Extension methods for commandline hosting.
    /// </summary>
    public static class CommandLineModuleHostingExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder AddCommandLineWorker(this IHostBuilder hostBuilder) => hostBuilder.ConfigureServices(
            (hostContext, services) =>
            {
                services.AddHostedService<CommandLineWorker>();
            });

        /// <summary>
        /// Run with modules
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="suppressStatusMessages"></param>
        /// <returns></returns>
        public static async Task RunConsoleAsyncWithModules(this IHostBuilder hostBuilder, bool? suppressStatusMessages = null)
        {
            hostBuilder = hostBuilder.AddCommandLineWorker();

            if (suppressStatusMessages is true)
            {
                hostBuilder.ConfigureLogging(builder =>
                {
                    builder.AddFilter("Modulight.Modules.Hosting", LogLevel.Warning);
                });
            }

            hostBuilder = suppressStatusMessages is null ? hostBuilder.UseConsoleLifetime() :
                hostBuilder.UseConsoleLifetime(o => o.SuppressStatusMessages = suppressStatusMessages.Value);

            var host = hostBuilder.Build();

            await using var _ = await host.Services.UseModules();

            await host.RunAsync();
        }
    }
}