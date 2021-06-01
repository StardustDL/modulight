using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task RunAsyncWithModules(this IHost host, CancellationToken cancellationToken = default)
        {
            await using var _ = await host.Services.UseModules(cancellationToken).ConfigureAwait(false);

            await host.RunAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
