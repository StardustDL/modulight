using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Modulight.Modules.Hosting
{

    /// <summary>
    /// Specifies the contract for module host builder.
    /// </summary>
    public interface IModuleHostBuilder
    {
        /// <summary>
        /// Get all registered modules.
        /// </summary>
        IReadOnlyList<Type> Modules { get; }

        /// <summary>
        /// Get all registered plugins.
        /// </summary>
        IReadOnlyList<Type> Plugins { get; }

        /// <summary>
        /// Add module in given type.
        /// </summary>
        /// <param name="type">Module type.</param>
        /// <returns></returns>
        IModuleHostBuilder AddModule(Type type);

        /// <summary>
        /// Add plugin in given type.
        /// </summary>
        /// <param name="type">Plugin type.</param>
        /// <returns></returns>
        IModuleHostBuilder UsePlugin(Type type);

        /// <summary>
        /// Configure the services for the builder, also for all <see cref="IModuleHostBuilderPlugin"/> and <see cref="IModuleStartup"/>.
        /// </summary>
        /// <param name="configureBuilderServices"></param>
        /// <returns></returns>
        IModuleHostBuilder ConfigureBuilderServices(Action<IServiceCollection> configureBuilderServices);

        /// <summary>
        /// Configure the services for the target <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="configureServices"></param>
        /// <returns></returns>
        IModuleHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        /// <summary>
        /// Build the modules and the module host in service collection.
        /// </summary>
        /// <param name="services">Target service collection.</param>
        /// <param name="builderServices">Service collection for builder.</param>
        void Build(IServiceCollection services, IServiceCollection? builderServices = null);
    }
}