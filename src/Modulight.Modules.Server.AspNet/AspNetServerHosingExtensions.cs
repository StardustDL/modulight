using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for Asp.NET hosting.
    /// </summary>
    public static class AspNetServerHosingExtensions
    {
        /// <summary>
        /// Run with modules
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static async Task RunAsyncWithModules(this IHost host)
        {
            await using var _ = await host.Services.UseModules();

            await host.RunAsync();
        }
    }
}
