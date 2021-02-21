using Microsoft.Extensions.Logging;
using Modulight.Modules.Hosting;
using System;
using System.Threading.Tasks;

namespace Modulight.Modules
{
    /// <summary>
    /// Specifies the contract for modules.
    /// </summary>
    public interface IModule
    {
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
        /// Get the module manifest.
        /// </summary>
        ModuleManifest Manifest { get; }
    }

    /// <summary>
    /// Basic implementation for <see cref="IModule"/>, cooperated with <see cref="IModuleHost"/>.
    /// </summary>
    public abstract class Module : IModule
    {
        Lazy<ModuleManifest> _manifest;

        /// <summary>
        /// Get the module host.
        /// </summary>
        public IModuleHost Host { get; }

        /// <summary>
        /// Get the service provider.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Create module instance.
        /// </summary>
        /// <param name="host"></param>
        protected Module(IModuleHost host)
        {
            Host = host;
            Services = host.Services;
            _manifest = new Lazy<ModuleManifest>(() => Host.GetManifest(GetType()));
        }

        /// <inheritdoc/>
        public ModuleManifest Manifest => _manifest.Value;

        /// <inheritdoc/>
        public T GetService<T>(IServiceProvider provider) where T : notnull => Host.GetService<T>(provider, GetType());

        /// <inheritdoc/>
        public T GetOption<T>(IServiceProvider provider) where T : class => Host.GetOption<T>(provider, GetType());

        /// <inheritdoc/>
        public virtual Task Initialize() => Task.CompletedTask;

        /// <inheritdoc/>
        public virtual Task Shutdown() => Task.CompletedTask;
    }
}
