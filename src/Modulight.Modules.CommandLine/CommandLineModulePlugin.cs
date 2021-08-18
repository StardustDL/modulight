using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modulight.Modules.Hosting;

namespace Modulight.Modules.CommandLine
{
    internal sealed class CommandLineModulePlugin : ModuleHostBuilderPlugin
    {
        public CommandLineModulePlugin(IServiceProvider builderServices, IOptions<CommandLineModuleBuilderOptions> options)
        {
            BuilderServices = builderServices;
            Options = options.Value;
        }

        IServiceProvider BuilderServices { get; }

        public CommandLineModuleBuilderOptions Options { get; }

        public override void AfterBuild(ModuleDefinition[] modules, IServiceCollection services)
        {
            services.AddHostedService<CommandLineWorker>();

            if (Options.SuppressStatusMessages is true)
            {
                services.AddLogging(builder =>
                {
                    builder.AddFilter("Modulight.Modules.Hosting", LogLevel.Warning);
                });
            }

            // From HostBuilder.UseConsoleLifetime
            services.AddSingleton<IHostLifetime, ConsoleLifetime>();
            services.Configure<ConsoleLifetimeOptions>(o => o.SuppressStatusMessages = Options.SuppressStatusMessages);

            base.AfterBuild(modules, services);
        }

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
