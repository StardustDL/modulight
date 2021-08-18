using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;

namespace Modulight.Modules.CommandLine
{

    /// <summary>
    /// Specifies the contract for commandline modules.
    /// </summary>
    public interface ICommandLineModule : IModule
    {
        /// <summary>
        /// Get the manifest.
        /// </summary>
        CommandLineModuleManifest CommandLineModuleManifest { get; }
    }

    /// <summary>
    /// Basic implement for <see cref="ICommandLineModule"/>
    /// </summary>
    [ModuleDependency(typeof(CommandLineCoreModule))]
    public abstract class CommandLineModule : Module, ICommandLineModule
    {
        /// <summary>
        /// Get the collection.
        /// </summary>
        protected ICommandLineModuleCollection Collection { get; }

        readonly Lazy<CommandLineModuleManifest> _manifest;

        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected CommandLineModule(IModuleHost host) : base(host)
        {
            Collection = host.GetCommandLineModuleCollection();
            _manifest = new Lazy<CommandLineModuleManifest>(() => Collection.GetManifest(GetType()));
        }

        /// <inheritdoc/>
        public CommandLineModuleManifest CommandLineModuleManifest => _manifest.Value;
    }

    [Module(Author = "StardustDL", Description = "Provide services for commandline modules.", Url = "https://github.com/StardustDL/modulight")]
    [ModuleService(typeof(CommandLineModuleCollection), ServiceType = typeof(ICommandLineModuleCollection), Lifetime = ServiceLifetime.Singleton)]
    class CommandLineCoreModule : Module
    {
        public CommandLineCoreModule(IModuleHost host, ICommandLineModuleCollection collection) : base(host)
        {
            Collection = collection;
        }

        public ICommandLineModuleCollection Collection { get; }
    }
}
