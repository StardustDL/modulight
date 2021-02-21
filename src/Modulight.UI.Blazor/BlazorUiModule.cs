using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor.Services;
using StardustDL.RazorComponents.AntDesigns;
using StardustDL.RazorComponents.MaterialDesignIcons;
using System;

namespace Modulight.UI.Blazor
{
    /// <summary>
    /// Extension methods for default Blazor UI.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Add Blazor UI Module with customed UI provider.
        /// </summary>
        /// <typeparam name="TUIProvider"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IModuleHostBuilder AddBlazorUI<TUIProvider>(this IModuleHostBuilder builder) where TUIProvider : class, IBlazorUIProvider
        {
            return builder.ConfigureServices(sc =>
            {
                sc.AddScoped<IBlazorUIProvider, TUIProvider>();
            }).AddModule<BlazorUiModule>();
        }
    }

    /// <summary>
    /// Blazor UI Module
    /// </summary>
    [ModuleService(typeof(BlazorUIProvider), ServiceType = typeof(IBlazorUIProvider), RegisterBehavior = ServiceRegisterBehavior.Optional)]
    [Module(Url = "https://github.com/StardustDL/delights", Author = "StardustDL", Description = "Provide user interfaces for blazor client module hosting.")]
    [ModuleUIResource(UIResourceType.StyleSheet, "https://cdn.bootcdn.net/ajax/libs/twitter-bootstrap/4.5.3/css/bootstrap.min.css")]
    [ModuleUIResource(UIResourceType.Script, "https://cdn.bootcdn.net/ajax/libs/jquery/3.5.1/jquery.slim.min.js")]
    [ModuleDependency(typeof(AntDesignModule))]
    [ModuleDependency(typeof(MaterialDesignIconModule))]
    public class BlazorUiModule : RazorComponentClientModule
    {
        /// <summary>
        /// Create instance.
        /// </summary>
        /// <param name="host"></param>
        public BlazorUiModule(IModuleHost host) : base(host)
        {
        }
    }
}
