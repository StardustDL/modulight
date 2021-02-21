using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Default methods for module host builders.
    /// </summary>
    public static class ModuleHostBuilder
    {
        /// <summary>
        /// Create a defualt module host builder.
        /// </summary>
        /// <returns></returns>
        public static IModuleHostBuilder CreateDefaultBuilder()
        {
            return new DefaultModuleHostBuilder();
        }
    }

    /// <summary>
    /// Default implement for <see cref="IModuleHostBuilder"/>.
    /// </summary>
    class DefaultModuleHostBuilder : IModuleHostBuilder
    {
        /// <summary>
        /// Module descriptors for registered modules.
        /// </summary>
        protected List<Type> ModuleDescriptors { get; } = new List<Type>();

        /// <summary>
        /// Plugin descriptors for registered modules.
        /// </summary>
        protected List<Type> PluginDescriptors { get; } = new List<Type>();

        protected List<Action<IServiceCollection>> BuilderServiceConfiguration { get; } = new List<Action<IServiceCollection>>();

        protected List<Action<IServiceCollection>> ServiceConfiguration { get; } = new List<Action<IServiceCollection>>();

        /// <inheritdoc />
        public virtual IReadOnlyList<Type> Modules => ModuleDescriptors.AsReadOnly();

        /// <inheritdoc />
        public virtual IReadOnlyList<Type> Plugins => PluginDescriptors.AsReadOnly();

        /// <inheritdoc />
        public virtual IModuleHostBuilder AddModule(Type type)
        {
            type.EnsureModule();
            if (!ModuleDescriptors.Contains(type))
            {
                ModuleDescriptors.Add(type);
            }
            return this;
        }

        /// <inheritdoc />
        public virtual IModuleHostBuilder UsePlugin(Type type)
        {
            type.EnsureHostBuilderPlugin();
            if (!PluginDescriptors.Contains(type))
            {
                PluginDescriptors.Add(type);
            }
            return this;
        }

        protected virtual void BeforeBuild(IList<Type> modules, IServiceCollection services, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.BeforeBuild(modules, services);
            }
            services.AddLogging().AddOptions();
        }

        protected virtual void AfterBuild(IServiceCollection services, ModuleDefinition[] modules, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.AfterBuild(modules, services);
            }
        }

        protected virtual void BeforeModule(IServiceCollection services, ModuleDefinition module, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.BeforeModule(module, services);
            }
        }

        protected virtual void AfterModule(IServiceCollection services, ModuleDefinition module, IReadOnlyList<IModuleHostBuilderPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.AfterModule(module, services);
            }
        }

        /// <inheritdoc />
        public void Build(IServiceCollection services, IServiceCollection? builderServices = null)
        {
            static Type? GetStartupType(Type module)
            {
                var startupAttr = module.GetCustomAttribute<ModuleStartupAttribute>(true);
                if (startupAttr is not null)
                {
                    startupAttr.StartupType.EnsureModuleStartup();
                    return startupAttr.StartupType;
                }
                return null;
            }

            static ModuleDefinition ResolveModule(Type type, IServiceProvider serviceProvider)
            {
                var manifestBuilder = serviceProvider.GetRequiredService<IModuleManifestBuilder>();
                manifestBuilder.WithDefaultsFromModuleType(type);
                var startupType = GetStartupType(type);
                IModuleStartup? startup = null;
                if (startupType is not null)
                {
                    startup = (IModuleStartup)ActivatorUtilities.CreateInstance(serviceProvider, startupType);

                    startup.ConfigureManifest(manifestBuilder);
                }
                return new ModuleDefinition(type, manifestBuilder.Build(), startup);
            }

            static List<ModuleDefinition> ResolveModuleDependency(IEnumerable<Type> initialModules, IServiceProvider serviceProvider)
            {
                var result = new List<ModuleDefinition>();

                Dictionary<Type, IModuleStartup?> startups = new Dictionary<Type, IModuleStartup?>();
                Dictionary<Type, ModuleManifest> manifests = new Dictionary<Type, ModuleManifest>();
                Dictionary<Type, int> inDegrees = new Dictionary<Type, int>();

                Queue<Type> queue = new Queue<Type>();

                void addModule(Type type)
                {
                    var info = ResolveModule(type, serviceProvider);
                    startups.Add(type, info.Startup);
                    manifests.Add(type, info.Manifest);
                    inDegrees.Add(type, 0);
                }

                foreach (var t in initialModules.Reverse())
                {
                    addModule(t);
                    queue.Enqueue(t);
                }
                while (queue.Count > 0)
                {
                    var cur = queue.Dequeue();
                    var manifest = manifests[cur];
                    foreach (var dep in manifest.Dependencies)
                    {
                        if (!manifests.ContainsKey(dep))
                        {
                            addModule(dep);
                            queue.Enqueue(dep);
                        }
                        inDegrees[dep] += 1;
                    }
                }

                foreach (var item in inDegrees.Where(x => x.Value is 0))
                    queue.Enqueue(item.Key);

                while (queue.Count > 0)
                {
                    var cur = queue.Dequeue();
                    var manifest = manifests[cur];
                    result.Add(new ModuleDefinition(cur, manifest, startups[cur]));

                    foreach (var dep in manifest.Dependencies)
                    {
                        Debug.Assert(inDegrees[dep] >= 1, $"{dep.FullName} in-degree is 0.");

                        inDegrees[dep] -= 1;
                        if (inDegrees[dep] is 0)
                        {
                            queue.Enqueue(dep);
                        }
                    }
                }
                result.Reverse();
                return result;
            }

            builderServices ??= new ServiceCollection();
            foreach (var configure in BuilderServiceConfiguration)
            {
                configure(builderServices);
            }
            builderServices.TryAddTransient<IModuleManifestBuilder, DefaultModuleManifestBuilder>();
            builderServices.AddLogging().AddOptions();

            PluginDescriptors.ForEach(plugin => builderServices.AddSingleton(plugin));

            using var builderService = builderServices.BuildServiceProvider();
            var logger = builderService.GetRequiredService<ILogger<DefaultModuleHostBuilder>>();

            var plugins = new List<IModuleHostBuilderPlugin>();

            PluginDescriptors.ForEach(type =>
            {
                plugins.Add((IModuleHostBuilderPlugin)builderService.GetRequiredService(type));
                logger.LogInformation($"Loaded plugin {type.FullName}.");
            });

            foreach (var configure in ServiceConfiguration)
            {
                configure(services);
            }

            IList<Type> initialModules = new List<Type>(Modules.ToArray());

            BeforeBuild(initialModules, services, plugins);

            var modules = ResolveModuleDependency(initialModules, builderService);
            Dictionary<Type, Type> moduleStartups = new Dictionary<Type, Type>();

            foreach (var definition in modules)
            {
                var (type, manifest, startup) = definition;

                logger.LogInformation($"Processing module {type.FullName}.");

                if (startup is not null)
                {
                    startup.ConfigureServices(services);
                }

                BeforeModule(services, definition, plugins);

                services.AddSingleton(type);
                foreach (var service in manifest.Services)
                {
                    var des = new ServiceDescriptor(service.ServiceType, service.ImplementationType, service.Lifetime);
                    switch (service.RegisterBehavior)
                    {
                        case ServiceRegisterBehavior.Normal:
                            services.Add(des);
                            break;
                        case ServiceRegisterBehavior.Optional:
                            services.TryAdd(des);
                            break;
                    }
                }


                AfterModule(services, definition, plugins);

                logger.LogInformation($"Processed module {type.FullName}.");
            }

            AfterBuild(services, modules.ToArray(), plugins);

            var definedModules = modules.Select(x => (x.Type, x.Manifest)).ToArray();
            services.TryAddSingleton<IModuleHost>(sp => new DefaultModuleHost(sp, definedModules));
        }

        public IModuleHostBuilder ConfigureBuilderServices(Action<IServiceCollection> configureBuilderServices)
        {
            BuilderServiceConfiguration.Add(configureBuilderServices);
            return this;
        }

        public IModuleHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            ServiceConfiguration.Add(configureServices);
            return this;
        }
    }
}