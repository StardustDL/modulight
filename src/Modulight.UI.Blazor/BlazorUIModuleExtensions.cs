using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor;
using Modulight.UI.Blazor.Services;

namespace Modulight.Modules
{
    /// <summary>
    /// Extension methods for default Blazor UI.
    /// </summary>
    public static class BlazorUIModuleExtensions
    {
        /// <summary>
        /// Add Blazor UI Module with customed UI provider.
        /// </summary>
        /// <typeparam name="TUIProvider"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddBlazorUI<TUIProvider>(this IModuleHostBuilder builder) where TUIProvider : class, IBlazorUIProvider
        {
            return builder.ConfigureServices(sc => sc.AddScoped<IBlazorUIProvider, TUIProvider>()).AddModule<BlazorUiModule>();
        }
    }
}
