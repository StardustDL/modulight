using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Extension methods for Asp.NET hosting.
    /// </summary>
    public static class AspNetServerModuleHostingExtensions
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
