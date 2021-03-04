using Microsoft.Extensions.DependencyInjection;
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
    }

    internal class DefaultModuleHost : IModuleHost
    {
        IReadOnlyDictionary<Type, IModule> _LoadedModules { get; set; } = new Dictionary<Type, IModule>();

        IReadOnlyDictionary<Type, ModuleManifest> _DefinedModules { get; set; }

        public DefaultModuleHost(IServiceProvider services, (Type, ModuleManifest)[] definedModules)
        {
            Services = services;

            _DefinedModules = new Dictionary<Type, ModuleManifest>(definedModules.Select(x => new KeyValuePair<Type, ModuleManifest>(x.Item1, x.Item2)));
        }

        public virtual IEnumerable<IModule> LoadedModules => _LoadedModules.Values.AsEnumerable();

        public virtual IEnumerable<Type> DefinedModules => _DefinedModules.Keys.AsEnumerable();

        public virtual IServiceProvider Services { get; protected set; }

        public virtual ModuleManifest GetManifest(Type moduleType)
        {
            return _DefinedModules.TryGetValue(moduleType, out var value)
                ? value
                : throw new Exception($"No such defined module: {moduleType.FullName}.");
        }

        public virtual IModule GetModule(Type moduleType)
        {
            return _LoadedModules.TryGetValue(moduleType, out var value)
                ? value
                : throw new Exception($"No such loaded module: {moduleType.FullName}.");
        }

        public virtual async Task Initialize()
        {
            var modules = new List<(Type, IModule)>();
            foreach (var type in DefinedModules)
            {
                modules.Add((type, (IModule)Services.GetRequiredService(type)));
            }
            _LoadedModules = new Dictionary<Type, IModule>(modules.Select(x => new KeyValuePair<Type, IModule>(x.Item1, x.Item2)));

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

            _LoadedModules = new Dictionary<Type, IModule>();
        }
    }
}