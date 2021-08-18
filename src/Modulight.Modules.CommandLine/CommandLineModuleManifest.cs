namespace Modulight.Modules.CommandLine
{
    /// <summary>
    /// Manifest for <see cref="CommandLineModule"/>.
    /// </summary>
    public record CommandLineModuleManifest : ModuleManifest<ICommandLineModule>
    {
        /// <summary>
        /// Types for commands.
        /// </summary>
        public Type[] Commands { get; init; } = Array.Empty<Type>();
    }
}
