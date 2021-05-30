using Modulight.Modules.Hosting;

namespace Modulight.Modules.CommandLine
{
    /// <summary>
    /// Specifies the contract for commandline module hosts.
    /// </summary>
    public interface ICommandLineModuleCollection : IModuleCollection<ICommandLineModule, CommandLineModuleManifest>
    {
    }

    internal class CommandLineModuleCollection : ModuleHostFilterCollection<ICommandLineModule, CommandLineModuleManifest>, ICommandLineModuleCollection
    {
        public CommandLineModuleCollection(IModuleHost host) : base(host)
        {
        }
    }
}
