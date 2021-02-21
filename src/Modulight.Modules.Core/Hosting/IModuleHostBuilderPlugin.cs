using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Plugin for <see cref="IModuleHostBuilder"/>.
    /// </summary>
    public interface IModuleHostBuilderPlugin
    {
        /// <summary>
        /// Do after the host builded.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="services"></param>
        void AfterBuild(ModuleDefinition[] modules, IServiceCollection services);

        /// <summary>
        /// Do after the module registered.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="services"></param>
        void AfterModule(ModuleDefinition module, IServiceCollection services);

        /// <summary>
        /// Do before the host is building.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="services"></param>
        void BeforeBuild(IList<Type> modules, IServiceCollection services);

        /// <summary>
        /// Do before the module is registering.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="services"></param>
        void BeforeModule(ModuleDefinition module, IServiceCollection services);
    }

    /// <summary>
    /// Empty implementation for <see cref="IModuleHostBuilderPlugin"/>.
    /// </summary>
    public abstract class ModuleHostBuilderPlugin : IModuleHostBuilderPlugin
    {
        /// <inheritdoc/>
        public virtual void BeforeBuild(IList<Type> modules, IServiceCollection services) { }

        /// <inheritdoc/>
        public virtual void AfterBuild(ModuleDefinition[] modules, IServiceCollection services) { }

        /// <inheritdoc/>
        public virtual void BeforeModule(ModuleDefinition module, IServiceCollection services) { }

        /// <inheritdoc/>
        public virtual void AfterModule(ModuleDefinition module, IServiceCollection services) { }
    }
}