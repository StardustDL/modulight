using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor.Services;
using StardustDL.RazorComponents.AntDesigns;
using StardustDL.RazorComponents.Bootstraps;
using StardustDL.RazorComponents.MaterialDesignIcons;

namespace Modulight.UI.Blazor
{
    /// <summary>
    /// Blazor UI Module
    /// </summary>
    [ModuleService(typeof(BlazorUIProvider), ServiceType = typeof(IBlazorUIProvider), RegisterBehavior = ServiceRegisterBehavior.Optional)]
    [Module(Url = "https://github.com/StardustDL/delights", Author = "StardustDL", Description = "Provide user interfaces for blazor client module hosting.")]
    [ModuleOption(typeof(BlazorUiModuleOption))]
    [ModuleDependency(typeof(BootstrapModule))]
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

    /// <summary>
    /// Options for <see cref="BlazorUiModule"/>.
    /// </summary>
    public class BlazorUiModuleOption
    {

        /// <summary>
        /// Render UI resources, default to true.
        /// </summary>
        public bool RenderUIResources { get; set; } = true;
    }
}
