using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modulight.Modules.Hosting;

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
        Task Initialize(CancellationToken cancellationToken = default);

        /// <summary>
        /// Shutdown the module.
        /// </summary>
        /// <returns></returns>
        Task Shutdown(CancellationToken cancellationToken = default);

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
        readonly Lazy<ModuleManifest> _manifest;

        /// <summary>
        /// Get the logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Get the module host.
        /// </summary>
        protected IModuleHost Host { get; }

        /// <summary>
        /// Get the service provider.
        /// </summary>
        protected IServiceProvider Services { get; }

        /// <summary>
        /// Create module instance.
        /// </summary>
        /// <param name="host"></param>
        protected Module(IModuleHost host)
        {
            Host = host;
            Services = host.Services;
            Logger = Services.GetRequiredService<ILogger<Module>>();
            _manifest = new Lazy<ModuleManifest>(() => Host.GetManifest(GetType()));
        }

        /// <inheritdoc/>
        public ModuleManifest Manifest => _manifest.Value;

        /// <inheritdoc/>
        protected T GetService<T>(IServiceProvider provider) where T : notnull => Host.GetService<T>(provider, GetType());

        /// <inheritdoc/>
        protected T GetOption<T>(IServiceProvider provider) where T : class => Host.GetOption<T>(provider, GetType());

        /// <inheritdoc/>
        public virtual Task Initialize(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug($"Module Initialized: {Manifest.FullName}.");
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual Task Shutdown(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug($"Module Shutdowned: {Manifest.FullName}.");
            return Task.CompletedTask;
        }
    }
}
