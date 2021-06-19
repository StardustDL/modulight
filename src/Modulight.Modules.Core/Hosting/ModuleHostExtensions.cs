using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for module host.
    /// </summary>
    public static class ModuleHostExtensions
    {
        /// <summary>
        /// Add Modulight module services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureBuilder"></param>
        /// <returns></returns>
        public static IServiceCollection AddModules(this IServiceCollection services, Action<IModuleHostBuilder>? configureBuilder = null)
        {
            var builder = ModuleHostBuilder.CreateDefaultBuilder();
            if (configureBuilder is not null)
                configureBuilder(builder);
            return services.AddModules(builder);
        }

        /// <summary>
        /// Add Modulight module services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection AddModules(this IServiceCollection services, IModuleHostBuilder builder)
        {
            builder.Build(services);
            return services;
        }

        /// <summary>
        /// Get default module host from service provider.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IModuleHost GetModuleHost(this IServiceProvider services)
        {
            return services.GetRequiredService<IModuleHost>();
        }

        /// <summary>
        /// Create a context to control module host initializing, shutdown.
        /// For example:
        /// <code>
        /// await using var _ = await services.UseModules();
        /// </code>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IAsyncDisposable> UseModules(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            var host = services.GetModuleHost();
            await host.Initialize(cancellationToken).ConfigureAwait(false);
            return new ModuleHostContext(host);
        }

        /// <summary>
        /// Register module manifest.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TManifest"></typeparam>
        /// <param name="services"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterModuleManifest<TModule, TManifest>(this IServiceCollection services, ModuleManifestServiceEntry<TModule, TManifest> entry) where TModule : IModule where TManifest : ModuleManifest<TModule>
        {
            services.AddSingleton(entry);
            return services;
        }

        /// <summary>
        /// Get all registered module manifests.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="TManifest"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IEnumerable<ModuleManifestServiceEntry<TModule, TManifest>> GetModuleManifests<TModule, TManifest>(this IServiceProvider services) where TModule : IModule where TManifest : ModuleManifest<TModule>
        {
            return services.GetServices<ModuleManifestServiceEntry<TModule, TManifest>>();
        }
    }
}

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extensions for module host.
    /// </summary>
    public static class ModuleHostExtensions
    {
        /// <summary>
        /// Get manifest for the module.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="host"></param>
        /// <returns></returns>
        public static ModuleManifest GetManifest<TModule>(this IModuleHost host) where TModule : IModule => host.GetManifest(typeof(TModule));

        /// <summary>
        /// Get service that belongs to the module.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <param name="provider"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static T GetService<T>(this IModuleHost host, IServiceProvider provider, Type moduleType) where T : notnull
        {
            var manifest = host.GetManifest(moduleType);
            var type = typeof(T);
            return manifest.Services.Any(x => x.ServiceType == type)
                ? provider.GetRequiredService<T>()
                : throw new ModuleNotFoundException($"No such service for the module {moduleType.FullName}: {type.FullName}.");
        }

        /// <summary>
        /// Get option that belongs to the module.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <param name="provider"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static T GetOption<T>(this IModuleHost host, IServiceProvider provider, Type moduleType) where T : class
        {
            var manifest = host.GetManifest(moduleType);
            var type = typeof(T);
            return manifest.Options.Any(x => x == type)
                ? provider.GetRequiredService<IOptionsSnapshot<T>>().Value
                : throw new ModuleNotFoundException($"No such option for the module {moduleType.FullName}: {type.FullName}.");
        }
        /// <summary>
        /// Get the module instance with module type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <returns></returns>
        public static T GetModule<T>(this IModuleHost host) where T : IModule => (T)host.GetModule(typeof(T));

        /// <summary>
        /// Get service that belongs to the module.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static T GetService<TModule, T>(this IModuleHost host, IServiceProvider provider) where T : notnull where TModule : IModule => host.GetService<T>(provider, typeof(TModule));

        /// <summary>
        /// Get option that belongs to the module.
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static T GetOption<TModule, T>(this IModuleHost host, IServiceProvider provider) where T : class where TModule : IModule => host.GetOption<T>(provider, typeof(TModule));

        /// <summary>
        /// Add a typed module.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <returns></returns>
        public static IModuleHostBuilder AddModule<T>(this IModuleHostBuilder builder) where T : IModule => builder.AddModule(typeof(T));

        /// <summary>
        /// Add a typed plugin.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UsePlugin<T>(this IModuleHostBuilder builder) where T : IModuleHostBuilderPlugin => builder.UsePlugin(typeof(T));

        /// <summary>
        /// Configure builder options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder ConfigureBuilderOptions<T>(this IModuleHostBuilder builder, Action<T, IServiceProvider> configureOptions) where T : class
        {
            return builder.ConfigureBuilderServices(services => services.AddOptions<T>().Configure(configureOptions));
        }

        /// <summary>
        /// Configure options for target services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IModuleHostBuilder ConfigureOptions<T>(this IModuleHostBuilder builder, Action<T, IServiceProvider> configureOptions) where T : class
        {
            return builder.ConfigureServices(services => services.AddOptions<T>().Configure(configureOptions));
        }

        internal static bool IsHostBuilderPlugin(this Type type) => type.IsAssignableTo(typeof(IModuleHostBuilderPlugin));

        internal static void EnsureHostBuilderPlugin(this Type type)
        {
            if (!type.IsHostBuilderPlugin())
                throw new IncompatibleTypeException(type, typeof(IModuleHostBuilderPlugin));
        }
    }
}