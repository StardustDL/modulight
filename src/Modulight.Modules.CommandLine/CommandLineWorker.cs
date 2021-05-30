using CliFx;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Modulight.Modules.CommandLine
{
    class CommandLineWorker : BackgroundService
    {
        public CommandLineWorker(IServiceProvider services, IHostApplicationLifetime lifetime, ICommandLineModuleCollection collection)
        {
            Services = services;
            Lifetime = lifetime;
            Collection = collection;
        }

        IServiceProvider Services { get; }

        IHostApplicationLifetime Lifetime { get; }

        ICommandLineModuleCollection Collection { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            var app = new CliApplicationBuilder();

            {
                HashSet<Type> addedCommands = new HashSet<Type>();

                foreach (var module in Collection.LoadedModules)
                {
                    var cmanifest = Collection.GetManifest(module.GetType());
                    foreach (var cmd in cmanifest.Commands)
                    {
                        if (addedCommands.Contains(cmd))
                            continue;
                        app.AddCommand(cmd);

                        addedCommands.Add(cmd);
                    }
                }
            }

            var exitCode = await app
                .UseTypeActivator(scope.ServiceProvider.GetRequiredService)
                .Build()
                .RunAsync();

            Environment.ExitCode = exitCode;

            Lifetime.StopApplication();
        }
    }
}
