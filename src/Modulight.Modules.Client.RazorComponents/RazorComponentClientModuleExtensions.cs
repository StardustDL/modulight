using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Extension methods for razor component modules.
    /// </summary>
    public static class RazorComponentClientModuleExtensions
    {
        /// <summary>
        /// Get razor component module host from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleCollection GetRazorComponentClientModuleCollection(this IServiceProvider provider) => provider.GetRequiredService<IRazorComponentClientModuleCollection>();
    }
}
