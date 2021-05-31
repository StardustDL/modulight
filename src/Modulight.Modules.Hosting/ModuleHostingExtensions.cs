using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Extension methods for module hosting.
    /// </summary>
    public static class ModuleHostingExtensions
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
