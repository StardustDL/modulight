using Microsoft.Extensions.DependencyInjection;

namespace Modulight.Modules
{
    /// <summary>
    /// Startup for module (needs to be no-state, no disposable).
    /// </summary>
    public interface IModuleStartup
    {
        /// <summary>
        /// Configure the target services.
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Configure the module manifest.
        /// </summary>
        /// <param name="builder"></param>
        void ConfigureManifest(IModuleManifestBuilder builder);
    }

    /// <summary>
    /// Empty implementation for <see cref="IModuleStartup"/>.
    /// </summary>
    public abstract class ModuleStartup : IModuleStartup
    {
        /// <inheritdoc/>
        public virtual void ConfigureManifest(IModuleManifestBuilder builder) { }

        /// <inheritdoc/>
        public virtual void ConfigureServices(IServiceCollection services) { }
    }
}
