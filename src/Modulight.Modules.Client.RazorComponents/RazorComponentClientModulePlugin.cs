using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;

namespace Modulight.Modules.Client.RazorComponents
{
    internal record ModuleManifestItem(Type Type, RazorComponentClientModuleManifest Manifest);

    internal sealed class RazorComponentClientModulePlugin : ModuleHostBuilderPlugin
    {
        public override void BeforeBuild(IList<Type> modules, IServiceCollection services, IServiceProvider builderServices)
        {

            base.BeforeBuild(modules, services, builderServices);
        }

        public override void AfterModule(ModuleDefinition module, IServiceCollection services, IServiceProvider builderServices)
        {
            if (module.Type.IsModule<IRazorComponentClientModule>())
            {
                var manifestBuilder = builderServices.GetRequiredService<IRazorComponentClientModuleManifestBuilder>();
                manifestBuilder.WithDefaultsFromModuleType(module.Type);

                if (module.Startup is IRazorComponentClientModuleStartup startup)
                {
                    startup.ConfigureRazorComponentClientModuleManifest(manifestBuilder);
                }

                var manifest = manifestBuilder.Build();

                services.AddSingleton(new ModuleManifestItem(module.Type, manifest));
            }
            base.AfterModule(module, services, builderServices);
        }
    }

}