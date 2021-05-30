using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Modulight.Modules.CommandLine;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;

namespace Modulight.Modules.CommandLine
{
    internal sealed class CommandLineModulePlugin : ModuleHostBuilderPlugin
    {
        public CommandLineModulePlugin(IServiceProvider builderServices) => BuilderServices = builderServices;

        IServiceProvider BuilderServices { get; }

        public override void AfterModule(ModuleDefinition module, IServiceCollection services)
        {
            if (module.Type.IsModule<ICommandLineModule>())
            {
                var manifestBuilder = BuilderServices.GetRequiredService<ICommandLineModuleManifestBuilder>();
                manifestBuilder.WithDefaultsFromModuleType(module.Type);

                {
                    if (module.Startup is ICommandLineModuleStartup startup)
                    {
                        startup.ConfigureCommandLineModuleManifest(manifestBuilder);
                    }
                }

                var manifest = manifestBuilder.Build();

                services.RegisterModuleManifest(new ModuleManifestServiceEntry<ICommandLineModule, CommandLineModuleManifest>(module.Type, manifest));

                foreach (var cmd in manifest.Commands)
                    services.TryAddScoped(cmd);
            }
            base.AfterModule(module, services);
        }
    }
}
