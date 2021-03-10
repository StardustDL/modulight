using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Client.RazorComponents
{
    internal sealed class RazorComponentClientModulePlugin : ModuleHostBuilderPlugin
    {
        public RazorComponentClientModulePlugin(IServiceProvider builderServices) => BuilderServices = builderServices;

        IServiceProvider BuilderServices { get; }

        public override void AfterModule(ModuleDefinition module, IServiceCollection services)
        {
            if (module.Type.IsModule<IRazorComponentClientModule>())
            {
                var manifestBuilder = BuilderServices.GetRequiredService<IRazorComponentClientModuleManifestBuilder>();
                manifestBuilder.WithDefaultsFromModuleType(module.Type);

                if (module.Startup is IRazorComponentClientModuleStartup startup)
                {
                    startup.ConfigureRazorComponentClientModuleManifest(manifestBuilder);
                }

                var manifest = manifestBuilder.Build();

                if (manifest.PageProvider is not null)
                {
                    manifest.PageProvider.EnsurePageProvider();
                    services.AddScoped(manifest.PageProvider);
                }

                services.RegisterModuleManifest(new ModuleManifestServiceEntry<IRazorComponentClientModule, RazorComponentClientModuleManifest>(module.Type, manifest));
            }
            base.AfterModule(module, services);
        }
    }

}