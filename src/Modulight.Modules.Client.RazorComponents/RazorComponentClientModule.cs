using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies the contract for razor component modules.
    /// </summary>
    public interface IRazorComponentClientModule : IModule
    {
        /// <summary>
        /// Get module icon.
        /// </summary>
        RenderFragment? Icon { get; }

        /// <summary>
        /// Get the manifest.
        /// </summary>
        RazorComponentClientModuleManifest RazorComponentClientModuleManifest { get; }
    }

    /// <summary>
    /// Basic implement for <see cref="IRazorComponentClientModule"/>.
    /// </summary>
    [ModuleDependency(typeof(RazorComponentClientCoreModule))]
    public abstract class RazorComponentClientModule : Module, IRazorComponentClientModule
    {
        readonly Lazy<RazorComponentClientModuleManifest> _manifest;

        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected RazorComponentClientModule(IModuleHost host) : base(host)
        {
            Collection = host.GetRazorComponentClientModuleCollection();
            _manifest = new Lazy<RazorComponentClientModuleManifest>(() => Collection.GetManifest(GetType()));
        }

        /// <inheritdoc/>
        public virtual RenderFragment? Icon => null;

        /// <inheritdoc/>
        public RazorComponentClientModuleManifest RazorComponentClientModuleManifest => _manifest.Value;

        /// <summary>
        /// Get collection of razor component modules.
        /// </summary>
        protected IRazorComponentClientModuleCollection Collection { get; }

        /// <summary>
        /// Get page provider.
        /// </summary>
        /// <returns></returns>
        protected IPageProvider? GetPageProvider() => this.GetPageProvider(Host);
    }

    [Module(Author = "StardustDL", Description = "Provide services for razor component client modules.", Url = "https://github.com/StardustDL/modulight")]
    [ModuleService(typeof(RazorComponentClientModuleCollection), ServiceType = typeof(IRazorComponentClientModuleCollection), Lifetime = ServiceLifetime.Singleton)]
    [ModuleService(typeof(JSModuleProvider<>), ServiceType = typeof(IJSModuleProvider<>))]
    [ModuleService(typeof(LazyAssemblyLoader))]
    [ModuleService(typeof(ModuleUILoader))]
    class RazorComponentClientCoreModule : Module
    {
        public RazorComponentClientCoreModule(IModuleHost host, IRazorComponentClientModuleCollection collection) : base(host)
        {
            Collection = collection;
        }

        public IRazorComponentClientModuleCollection Collection { get; }

        public override async Task Initialize()
        {
            if (Environment.OSVersion.Platform is PlatformID.Other)
            {
                await Collection.LoadResources();
            }
            await base.Initialize();
        }
    }
}
