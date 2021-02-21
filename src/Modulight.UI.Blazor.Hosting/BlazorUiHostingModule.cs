using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.AspNet;
using Modulight.UI.Blazor.Services;
using System;

namespace Modulight.UI.Blazor.Hosting
{
    /// <summary>
    /// Extension methods for default Blazor UI.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add Blazor UI Hosting Module.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddBlazorUIHostingModule(this IModuleHostBuilder builder, Action<BlazorUiHostingModuleOption, IServiceProvider>? configureOptions = null)
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
            }).AddBlazorUI<TUIProvider>().AddBlazorUIHostingModule(
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
            return builder.ConfigureServices(services =>
            {
                services.AddRazorPages();
            }).AddBlazorUI<TUIProvider>().AddBlazorUIHostingModule(
                    (o, sp) =>
                    {
                        o.HostingModel = HostingModel.Client;
                        if (configureOptions is not null)
                            configureOptions(o, sp);
                    });
        }
    }

    /// <summary>
    /// Blazor UI Hosting Module
    /// </summary>
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(BlazorUiModule))]
    [ModuleOption(typeof(BlazorUiHostingModuleOption))]
    public class BlazorUiHostingModule : AspNetServerModule
    {
        /// <summary>
        /// Create instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="options"></param>
        public BlazorUiHostingModule(IModuleHost host, IOptions<BlazorUiHostingModuleOption> options) : base(host)
        {
            Options = options.Value;
        }

        /// <summary>
        /// Options
        /// </summary>
        public BlazorUiHostingModuleOption Options { get; }

        /// <inheritdoc/>
        public override void UseMiddleware(IApplicationBuilder builder)
        {
            if (Options.HostingModel is HostingModel.Client && Options.DefaultBlazorFrameworkFiles)
            {
                builder.UseBlazorFrameworkFiles();
            }
            base.UseMiddleware(builder);
        }

        /// <inheritdoc/>
        public override void MapEndpoint(IEndpointRouteBuilder builder)
        {
            if (Options.HostingModel is HostingModel.Server && Options.DefaultBlazorHub)
            {
                builder.MapBlazorHub();
            }
            builder.MapFallbackToAreaPage("/_Host", "Modulights");
            base.MapEndpoint(builder);
        }
    }

    class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
            base.ConfigureServices(services);
        }
    }
}
