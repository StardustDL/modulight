using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Filter modules in exact type.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public class ModuleHostFilter<TModule> : IModuleCollection<TModule> where TModule : IModule
    {
        /// <summary>
        /// Create the filter instance.
        /// </summary>
        /// <param name="host"></param>
        public ModuleHostFilter(IModuleHost host)
        {
            Host = host;
        }

        /// <summary>
        /// The inner <see cref="IModuleHost"/>.
        /// </summary>
        public IModuleHost Host { get; }

        /// <inheritdoc/>
        public IEnumerable<TModule> LoadedModules => Host.LoadedModules.Where(x => x is TModule).Select(x => (TModule)x);

        /// <inheritdoc/>
        public IEnumerable<Type> DefinedModules => Host.DefinedModules.Where(x => x.IsModule<TModule>());
    }
}