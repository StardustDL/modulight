using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor.Hosting;
using Modulight.UI.Blazor.Services;
using System;

namespace Modulight.Modules
{
    /// <summary>
    /// Extension methods for default Blazor UI.
    /// </summary>
    public static class BlazorUIHostingModuleExtensions
    {
        /// <summary>
        /// Add Blazor UI Hosting Module.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddBlazorUIHosting(this IModuleHostBuilder builder, Action<BlazorUiHostingModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<BlazorUiHostingModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureOptions(configureOptions);
            }
            return builder;
        }

        /// <summary>
        /// Add default server side blazor UI.
        /// </summary>
        /// <typeparam name="TUIProvider"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddServerSideBlazorUI<TUIProvider>(this IModuleHostBuilder builder, Action<BlazorUiHostingModuleOption, IServiceProvider>? configureOptions = null) where TUIProvider : class, IBlazorUIProvider
        {
            return builder.ConfigureServices(services =>
            {
                services.AddRazorPages();
                services.AddServerSideBlazor();
            }).AddBlazorUI<TUIProvider>((o, sp) =>
            {
                // Disable inner render, because of the server render.
                o.RenderUIResources = false;
            }).AddBlazorUIHosting(
                    (o, sp) =>
                    {
                        o.HostingModel = HostingModel.Server;
                        if (configureOptions is not null)
                            configureOptions(o, sp);
                    });
        }

        /// <summary>
        /// Add default server hosting client side blazor UI.
        /// </summary>
        /// <typeparam name="TUIProvider"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddClientSideBlazorUI<TUIProvider>(this IModuleHostBuilder builder, Action<BlazorUiHostingModuleOption, IServiceProvider>? configureOptions = null) where TUIProvider : class, IBlazorUIProvider
        {
            return builder.ConfigureServices(services => services.AddRazorPages()).AddBlazorUI<TUIProvider>((o, sp) =>
            {
                // Disable inner render, because of the server render.
                o.RenderUIResources = false;
            }).AddBlazorUIHosting(
                    (o, sp) =>
                    {
                        o.HostingModel = HostingModel.Client;
                        if (configureOptions is not null)
                            configureOptions(o, sp);
                    });
        }
    }
}
