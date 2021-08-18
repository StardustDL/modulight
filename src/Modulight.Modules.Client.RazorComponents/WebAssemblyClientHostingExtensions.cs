using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting
{
    /// <summary>
    /// Extension methods for WebAssembly hosting.
    /// </summary>
    public static class WebAssemblyClientModuleHostingExtensions
    {
        /// <summary>
        /// Run with modules
        /// </summary>
        /// <param name="host"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task RunAsyncWithModules(this WebAssemblyHost host, CancellationToken cancellationToken = default)
        {
            await using var _ = await host.Services.UseModules(cancellationToken).ConfigureAwait(false);

            await host.RunAsync().ConfigureAwait(false);
        }
    }
}
