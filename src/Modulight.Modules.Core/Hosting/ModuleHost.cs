using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// A collection of typed modules.
    /// </summary>
    /// <typeparam name="TModule">Base module type.</typeparam>
    public interface IModuleCollection<TModule> where TModule : IModule
    {
        /// <summary>
        /// Get all loaded modules.
        /// </summary>
        IEnumerable<TModule> LoadedModules { get; }

        /// <summary>
        /// Get all defined module types.
        /// </summary>
        IEnumerable<Type> DefinedModules { get; }
    }

    /// <summary>
    /// Specifies the contract for module hosts.
    /// </summary>
    public interface IModuleHost : IModuleCollection<IModule>
    {
        /// <summary>
        /// Service provider.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Get manifest for the module.
        /// </summary>
        ModuleManifest GetManifest(Type moduleType);

        /// <summary>
        /// Get the module instance with module type.
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        IModule GetModule(Type moduleType);

        /// <summary>
        /// Initialize the module.
        /// </summary>
        /// <returns></returns>
        Task Initialize();

        /// <summary>
        /// Shutdown the module.
        /// </summary>
        /// <returns></returns>
        Task Shutdown();

        /// <summary>
        /// Get service that belongs to the module.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        T GetService<T>(IServiceProvider provider, Type moduleType) where T : notnull;

        /// <summary>
        /// Get option that belongs to the module.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        T GetOption<T>(IServiceProvider provider, Type moduleType) where T : class;

        /// <summary>
        /// Get logger for the module.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        ILogger<TModule> GetLogger<TModule>();
    }

    internal class DefaultModuleHost : IModuleHost
    {
        IReadOnlyDictionary<Type, IModule> _LoadedModules { get; set; } = new Dictionary<Type, IModule>();

        IReadOnlyDictionary<Type, ModuleManifest> _DefinedModules { get; set; }

        public DefaultModuleHost(IServiceProvider services, (Type, ModuleManifest)[] definedModules)
        {
            Services = services;

            _DefinedModules = new Dictionary<Type, ModuleManifest>(definedModules.Select(x => new KeyValuePair<Type, ModuleManifest>(x.Item1, x.Item2)));
            DefinedModules = definedModules.Select(x => x.Item1);
        }

        public virtual IEnumerable<IModule> LoadedModules { get; protected set; } = Array.Empty<IModule>();

        public virtual IEnumerable<Type> DefinedModules { get; protected set; }

        public virtual IServiceProvider Services { get; protected set; }

        public virtual ModuleManifest GetManifest(Type moduleType)
        {
            if (_DefinedModules.TryGetValue(moduleType, out var value))
            {
                return value;
            }
            else
            {
                throw new Exception($"No such defined module: {moduleType.FullName}.");
            }
        }

        public virtual IModule GetModule(Type moduleType)
        {
            if (_LoadedModules.TryGetValue(moduleType, out var value))
            {
                return value;
            }
            else
            {
                throw new Exception($"No such loaded module: {moduleType.FullName}.");
            }
        }

        public virtual T GetService<T>(IServiceProvider provider, Type moduleType) where T : notnull
        {
            var manifest = GetManifest(moduleType);
            var type = typeof(T);
            if (manifest.Services.Any(x => x.ServiceType == type))
            {
                return provider.GetRequiredService<T>();
            }
            else
            {
                throw new Exception($"No such service for the module {moduleType.FullName}: {type.FullName}.");
            }
        }

        public virtual T GetOption<T>(IServiceProvider provider, Type moduleType) where T : class
        {
            var manifest = GetManifest(moduleType);
            var type = typeof(T);
            if (manifest.Options.Any(x => x == type))
            {
                return provider.GetRequiredService<IOptionsSnapshot<T>>().Value;
            }
            else
            {
                throw new Exception($"No such option for the module {moduleType.FullName}: {type.FullName}.");
            }
        }

        public virtual async Task Initialize()
        {
            var modules = new List<(Type, IModule)>();
            foreach (var type in DefinedModules)
            {
                modules.Add((type, (IModule)Services.GetRequiredService(type)));
            }
            _LoadedModules = new Dictionary<Type, IModule>(modules.Select(x => new KeyValuePair<Type, IModule>(x.Item1, x.Item2)));
            LoadedModules = modules.Select(x => x.Item2);

            foreach (var module in LoadedModules)
            {
                await module.Initialize();
            }
        }

        public virtual async Task Shutdown()
        {
            foreach (var module in LoadedModules.Reverse())
            {
                await module.Shutdown();
            }
        }

        public virtual ILogger<TModule> GetLogger<TModule>()
        {
            var type = typeof(TModule);
            if (_DefinedModules.ContainsKey(type))
            {
                return Services.GetRequiredService<ILogger<TModule>>();
            }
            else
            {
                throw new Exception($"No such module: {type.FullName}.");
            }
        }
    }
}