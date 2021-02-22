using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for WebAssembly hosting.
    /// </summary>
    public static class WebAssemblyClientHosingExtensions
    {
        /// <summary>
        /// Run with modules
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static async Task RunAsyncWithModules(this WebAssemblyHost host)
        {
            await using var _ = await host.Services.UseModules();

            await host.RunAsync();
        }
    }
}
