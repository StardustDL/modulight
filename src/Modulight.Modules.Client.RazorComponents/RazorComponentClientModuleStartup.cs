namespace Modulight.Modules.Client.RazorComponents
{

    /// <summary>
    /// Startup for graphql server module.
    /// </summary>
    public interface IRazorComponentClientModuleStartup : IModuleStartup
    {
        /// <summary>
        /// Configure manifest for razor component module.
        /// </summary>
        /// <param name="builder"></param>
        void ConfigureRazorComponentClientModuleManifest(IRazorComponentClientModuleManifestBuilder builder);
    }

    /// <summary>
    /// Empty implementation for <see cref="IRazorComponentClientModuleStartup"/>.
    /// </summary>
    public abstract class RazorComponentClientModuleStartup : ModuleStartup, IRazorComponentClientModuleStartup
    {
        /// <inheritdoc/>
        public virtual void ConfigureRazorComponentClientModuleManifest(IRazorComponentClientModuleManifestBuilder builder) { }
    }
}