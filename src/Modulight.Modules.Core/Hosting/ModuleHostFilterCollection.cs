using Microsoft.Extensions.DependencyInjection;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Filter modules in exact type.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public class ModuleHostFilterCollection<TModule> : IModuleCollection<TModule> where TModule : IModule
    {
        /// <summary>
        /// Create the filter instance.
        /// </summary>
        /// <param name="host"></param>
        public ModuleHostFilterCollection(IModuleHost host)
        {
            Host = host;
        }

        /// <summary>
        /// The inner <see cref="IModuleHost"/>.
        /// </summary>
        public IModuleHost Host { get; }

        /// <inheritdoc/>
        public virtual IEnumerable<TModule> LoadedModules => Host.LoadedModules.Where(x => x is TModule).Select(x => (TModule)x);

        /// <inheritdoc/>
        public virtual IEnumerable<Type> DefinedModules => Host.DefinedModules.Where(x => x.IsModule<TModule>());

        /// <inheritdoc/>
        public virtual TModule GetModule(Type moduleType) => LoadedModules.Where(x => x.GetType() == moduleType).First();
    }

    /// <summary>
    /// Filter modules in exact type.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    /// <typeparam name="TManifest"></typeparam>
    public class ModuleHostFilterCollection<TModule, TManifest> : ModuleHostFilterCollection<TModule>, IModuleCollection<TModule, TManifest> where TModule : IModule where TManifest : ModuleManifest<TModule>
    {
        Dictionary<Type, TManifest> Manifests { get; } = new Dictionary<Type, TManifest>();

        /// <summary>
        /// Create the filter instance.
        /// </summary>
        /// <param name="host"></param>
        public ModuleHostFilterCollection(IModuleHost host) : base(host)
        {
            foreach (var item in host.Services.GetModuleManifests<TModule, TManifest>())
            {
                // TODO: check dup manifest 
                Manifests.Add(item.Type, item.Manifest);
            }
        }

        /// <inheritdoc/>
        public TManifest GetManifest(Type moduleType) => Manifests[moduleType];
    }
}