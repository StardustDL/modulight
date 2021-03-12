using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor;
using Modulight.UI.Blazor.Services;
using System;

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
        public static IModuleHostBuilder AddBlazorUI<TUIProvider>(this IModuleHostBuilder builder, Action<BlazorUiModuleOption, IServiceProvider>? configureOptions = null) where TUIProvider : class, IBlazorUIProvider
        {
            builder.ConfigureServices(sc => sc.AddScoped<IBlazorUIProvider, TUIProvider>()).AddModule<BlazorUiModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureOptions(configureOptions);
            }
            return builder;
        }
    }
}
