namespace Modulight.Modules.CommandLine
{
    /// <summary>
    /// Startup for commandline module.
    /// </summary>
    public interface ICommandLineModuleStartup : IModuleStartup
    {
        /// <summary>
        /// Configure manifest for commandline module.
        /// </summary>
        /// <param name="builder"></param>
        void ConfigureCommandLineModuleManifest(ICommandLineModuleManifestBuilder builder);
    }

    /// <summary>
    /// Empty implementation for <see cref="ICommandLineModuleStartup"/>.
    /// </summary>
    public abstract class CommandLineModuleStartup : ModuleStartup, ICommandLineModuleStartup
    {

        /// <inheritdoc/>
        public virtual void ConfigureCommandLineModuleManifest(ICommandLineModuleManifestBuilder builder) { }
    }
}