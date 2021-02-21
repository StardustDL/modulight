using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulight.Modules.Test.Context
{
    class ModuleHostBuilderCollectorOption
    {
        public Dictionary<Type, Type> StartupChecking { get; set; } = new Dictionary<Type, Type>();
    }

    class ModuleHostBuilderCollector : ModuleHostBuilderPlugin
    {
        public ModuleHostBuilderCollector(IOptions<ModuleHostBuilderCollectorOption> options) => Options = options.Value;

        public ModuleHostBuilderCollectorOption Options { get; }

        public List<Type> ModuleProcessingOrder { get; } = new List<Type>();

        public override void BeforeModule(ModuleDefinition module, IServiceCollection services)
        {
            ModuleProcessingOrder.Add(module.Type);
            base.BeforeModule(module, services);
        }

        public override void AfterModule(ModuleDefinition module, IServiceCollection services)
        {
            if (Options.StartupChecking.TryGetValue(module.Type, out var startupType))
            {
                Assert.IsNotNull(module.Startup, "The startup is unexpected null.");
                Assert.AreEqual(startupType, module.Startup!.GetType(), "Startup type is not the same.");
            }
            base.AfterModule(module, services);
        }

        public override void AfterBuild(ModuleDefinition[] modules, IServiceCollection services)
        {
            services.AddSingleton(new ModuleHostBuilderLog
            {
                ModuleProcessingOrder = ModuleProcessingOrder.ToArray()
            });
            base.AfterBuild(modules, services);
        }
    }

    public class ModuleTestContext
    {
        protected IModuleHostBuilder Builder { get; }

        ModuleHostBuilderCollectorOption CollectorOption { get; } = new ModuleHostBuilderCollectorOption();

        public ModuleTestContext()
        {
            Builder = ModuleHostBuilder.CreateDefaultBuilder();
            WithPlugin<ModuleHostBuilderCollector>();
        }

        public ModuleTestContext WithModule<T>() where T : IModule
        {
            Builder.AddModule<T>();
            return this;
        }

        public ModuleTestContext WithPlugin<T>() where T : IModuleHostBuilderPlugin
        {
            Builder.UsePlugin<T>();
            return this;
        }

        public ModuleTestContext ConfigureBuilder(Action<IModuleHostBuilder> builder)
        {
            builder(Builder);
            return this;
        }

        public ModuleTestContext CheckStartup(Type moduleType, Type startupType)
        {
            moduleType.EnsureModule();
            startupType.EnsureModuleStartup();
            CollectorOption.StartupChecking.Add(moduleType, startupType);
            return this;
        }

        public ModuleTestContext CheckStartup<TModule, TStartup>() where TModule : IModule where TStartup : IModuleStartup => CheckStartup(typeof(TModule), typeof(TStartup));

        public async Task BuildAsync(Func<IModuleHost, Task> action)
        {
            var services = new ServiceCollection();
            var builderServices = new ServiceCollection();
            builderServices.AddOptions<ModuleHostBuilderCollectorOption>().Configure(o =>
            {
                o.StartupChecking = CollectorOption.StartupChecking;
            });
            Builder.Build(services, builderServices);
            await using var provider = services.BuildServiceProvider();
            await action(provider.GetModuleHost());
        }

        public async Task UseHostAsync(Func<IModuleHost, Task>? action = null)
        {
            await BuildAsync(async host =>
            {
                if (action is not null)
                    await action(host);
            });
        }

        public async Task UseHost(Action<IModuleHost>? action = null)
        {
            await BuildAsync(host =>
               {
                   if (action is not null)
                       action(host);
                   return Task.CompletedTask;
               });
        }

        public async Task Run(Action<IModuleHost>? action = null, Action<IModuleHost>? afterAction = null)
        {
            await UseHostAsync(async host =>
            {
                await using (var _ = await host.Services.UseModuleHost())
                {
                    if (action is not null)
                        action(host);
                }
                if (afterAction is not null)
                    afterAction(host);
            });
        }

        public async Task RunAsync(Func<IModuleHost, Task>? action = null, Action<IModuleHost>? afterAction = null)
        {
            await UseHostAsync(async host =>
            {
                await using (var _ = await host.Services.UseModuleHost())
                {
                    if (action is not null)
                        await action(host);
                }
                if (afterAction is not null)
                    afterAction(host);
            });
        }
    }

    public class ModuleTestContext<T> : ModuleTestContext where T : IModule
    {
        public ModuleTestContext() : base()
        {
            WithModule<T>();
        }
    }
}
