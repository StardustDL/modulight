using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Client.RazorComponents
{
    internal record ModuleManifestItem(Type Type, RazorComponentClientModuleManifest Manifest);

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

                services.AddSingleton(new ModuleManifestItem(module.Type, manifest));
            }
            base.AfterModule(module, services);
        }
    }

}