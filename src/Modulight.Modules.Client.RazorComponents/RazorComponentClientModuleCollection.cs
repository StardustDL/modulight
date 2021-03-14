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
        /// <returns></returns>
        Task Validate();

        /// <summary>
        /// Load all <see cref="UIResource"/> defined in modules into DOM.
        /// </summary>
        /// <returns></returns>
        Task LoadResources(Type? moduleType = null);
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
        /// <returns></returns>
        public static Task LoadResources<T>(this IRazorComponentClientModuleCollection collection) where T : IModule => collection.LoadResources(typeof(T));
    }

    internal class RazorComponentClientModuleCollection : ModuleHostFilterCollection<IRazorComponentClientModule, RazorComponentClientModuleManifest>, IRazorComponentClientModuleCollection
    {
        Dictionary<Type, RazorComponentClientModuleManifest> Manifests { get; } = new Dictionary<Type, RazorComponentClientModuleManifest>();

        public RazorComponentClientModuleCollection(IModuleHost host) : base(host)
        {
            Logger = host.Services.GetRequiredService<ILogger<RazorComponentClientModuleCollection>>();
        }

        public ILogger<RazorComponentClientModuleCollection> Logger { get; }

        public async Task LoadResources(Type? moduleType = null)
        {
            using var scope = Host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            var ui = provider.GetRequiredService<ModuleUILoader>();

            var targetModules = LoadedModules;
            if (moduleType is not null)
            {
                targetModules = targetModules.Where(x => x.GetType().IsModule(moduleType));
            }

            foreach (var manifest in Manifests.Values)
            {
                foreach (var resource in manifest.Resources)
                {
                    try
                    {
                        switch (resource.Type)
                        {
                            case UIResourceType.Script:
                                await ui.LoadScript(resource.Path);
                                break;
                            case UIResourceType.StyleSheet:
                                await ui.LoadStyleSheet(resource.Path);
                                break;
                        }
                    }
                    catch (JSException ex)
                    {
                        Logger.LogError(ex, $"Failed to load resource {resource.Path}");
                    }
                }
            }
        }

        public async Task Validate()
        {
            using var scope = Host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            HashSet<string> rootPaths = new HashSet<string>();
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

                    await GetAssembliesForRouting($"/{pages.RootPath}", throwOnError: true);
                }
            }
        }

        public async Task<List<Assembly>> GetAssembliesForRouting(string path, bool recurse = false, bool throwOnError = false, CancellationToken cancellationToken = default)
        {
            using var scope = Host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            var lazyLoader = provider.GetRequiredService<LazyAssemblyLoader>();

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            HashSet<string> processed = new HashSet<string>();

            List<Assembly> results = new List<Assembly>();

            Queue<string> toLoad = new Queue<string>();

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
                            ? (await lazyLoader.LoadAssembliesAsync(new[] { current + ".dll" })).FirstOrDefault()
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