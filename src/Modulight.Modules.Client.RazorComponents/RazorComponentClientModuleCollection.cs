using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents
{

    /// <summary>
    /// Specifies the contract for razor component module hosts.
    /// </summary>
    public interface IRazorComponentClientModuleCollection : IModuleCollection<IRazorComponentClientModule, RazorComponentClientModuleManifest>
    {
        /// <summary>
        /// Load related assemblies for a given route.
        /// </summary>
        /// <param name="path">Route path.</param>
        /// <param name="recurse">Load dependent assemblies recursely.</param>
        /// <param name="throwOnError">Throw exceptions when error occurs instead of logs.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Assembly>> GetAssembliesForRouting(string path, bool recurse = false, bool throwOnError = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validate modules.
        /// Check if route roots conflict or assembly loading fails.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Validate(CancellationToken cancellationToken = default);

        /// <summary>
        /// Load all <see cref="UIResource"/> defined in modules into DOM.
        /// </summary>
        /// <param name="moduleType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task LoadResources(Type? moduleType = null, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Extension methods for <see cref="IRazorComponentClientModuleCollection"/>.
    /// </summary>
    public static class RazorComponentClientModuleCollectionExtensions
    {
        /// <summary>
        /// Load all <see cref="UIResource"/> defined in modules into DOM.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task LoadResources<T>(this IRazorComponentClientModuleCollection collection, CancellationToken cancellationToken = default) where T : IModule => collection.LoadResources(typeof(T), cancellationToken);
    }

    internal class RazorComponentClientModuleCollection : ModuleHostFilterCollection<IRazorComponentClientModule, RazorComponentClientModuleManifest>, IRazorComponentClientModuleCollection
    {
        public RazorComponentClientModuleCollection(IModuleHost host) : base(host)
        {
            Logger = host.Services.GetRequiredService<ILogger<RazorComponentClientModuleCollection>>();
        }

        public ILogger Logger { get; }

        public async Task LoadResources(Type? moduleType = null, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"Loading resources for {(moduleType is null ? "all" : moduleType.FullName)} modules.");

            using var scope = Host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            var ui = provider.GetRequiredService<ModuleUILoader>();

            var targetModules = DefinedModules;
            if (moduleType is not null)
            {
                targetModules = targetModules.Where(x => x.IsModule(moduleType));
            }

            foreach (var module in targetModules)
            {
                var manifest = Host.GetManifest(module);

                Logger.LogInformation($"Loading resources for {manifest.FullName}.");

                var rcmanifest = GetManifest(module);

                foreach (var resource in rcmanifest.Resources)
                {
                    try
                    {
                        switch (resource.Type)
                        {
                            case UIResourceType.Script:
                                Logger.LogDebug($"Load script {resource.Path} of {manifest.FullName}.");
                                await ui.LoadScript(resource.Path,cancellationToken).ConfigureAwait(false);
                                break;
                            case UIResourceType.StyleSheet:
                                Logger.LogDebug($"Load stylesheet {resource.Path} of {manifest.FullName}.");
                                await ui.LoadStyleSheet(resource.Path,cancellationToken).ConfigureAwait(false);
                                break;
                        }
                    }
                    catch (JSException ex)
                    {
                        Logger.LogError(ex, $"Failed to load resource {resource.Path} of {manifest.FullName}");
                    }
                }
            }

            Logger.LogInformation($"Loaded resources for {(moduleType is null ? "all" : moduleType.FullName)} modules.");
        }

        public async Task Validate(CancellationToken cancellationToken = default)
        {
            using var scope = Host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            HashSet<string> rootPaths = new();
            foreach (var module in LoadedModules)
            {
                var pages = module.GetPageProvider(Host);
                if (pages is not null)
                {
                    if (pages.RootPath is not "")
                    {
                        if (rootPaths.Contains(pages.RootPath))
                        {
                            throw new Exception($"Same RootPath in modules: {pages.RootPath} @ {module.Manifest.Name}");
                        }
                        rootPaths.Add(pages.RootPath);
                    }

                    await GetAssembliesForRouting($"/{pages.RootPath}", throwOnError: true,cancellationToken:cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public async Task<List<Assembly>> GetAssembliesForRouting(string path, bool recurse = false, bool throwOnError = false, CancellationToken cancellationToken = default)
        {
            using var scope = Host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            var lazyLoader = provider.GetRequiredService<LazyAssemblyLoader>();

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            HashSet<string> processed = new();

            List<Assembly> results = new();

            Queue<string> toLoad = new();

            foreach (var module in LoadedModules)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var name = module.GetType().GetAssemblyName();

                if (processed.Add(name))
                {
                    toLoad.Enqueue(name);
                }

                var pages = module.GetPageProvider(Host);

                if (pages is null || pages is not null && pages.Contains(path))
                {
                    foreach (var resource in GetManifest(module.GetType()).Resources.Where(x => x.Type is UIResourceType.Assembly))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (processed.Add(resource.Path))
                        {
                            toLoad.Enqueue(resource.Path);
                        }
                    }
                }
            }

            while (toLoad.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var current = toLoad.Dequeue();

                Assembly? assembly;

                assembly = loadedAssemblies.FirstOrDefault(x => x.GetName().Name == current);

                if (assembly is null)
                {
                    try
                    {
                        // Logger.LogInformation($"Loading assembly {current}");
                        assembly = Environment.OSVersion.Platform == PlatformID.Other
                            ? (await lazyLoader.LoadAssembliesAsync(new[] { current + ".dll" }).ConfigureAwait(false)).FirstOrDefault()
                            : Assembly.Load(current);
                    }
                    catch (Exception ex)
                    {
                        if (throwOnError)
                        {
                            throw;
                        }
                        else
                        {
                            Logger.LogWarning($"Failed to load assembly {current}: {ex}");
                        }
                    }
                }

                if (assembly is null)
                {
                    if (throwOnError)
                    {
                        throw new NullReferenceException($"Failed to load assembly {current}.");
                    }
                    Logger.LogError($"Failed to load assembly {current}.");
                    continue;
                }

                results.Add(assembly);

                if (recurse)
                {
                    foreach (var refe in assembly.GetReferencedAssemblies())
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        if (refe.Name is not null)
                        {
                            if (processed.Add(refe.Name))
                            {
                                toLoad.Enqueue(refe.Name);
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}