using Microsoft.Extensions.DependencyInjection;

namespace Modulight.Modules
{

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

    /// <summary>
    /// Service descriptor for module services.
    /// </summary>
    public record ModuleServiceDescriptor(Type ImplementationType, Type ServiceType, ServiceLifetime Lifetime = ServiceLifetime.Scoped, ServiceRegisterBehavior RegisterBehavior = ServiceRegisterBehavior.Normal)
    {
    }

    /// <summary>
    /// A entry in service collection for module manifest.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    /// <typeparam name="TManifest"></typeparam>
    public record ModuleManifestServiceEntry<TModule, TManifest>(Type Type, TManifest Manifest) where TModule : IModule where TManifest : ModuleManifest<TModule>
    {
    }

#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

    /// <summary>
    /// Generic manifest for modules
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public record ModuleManifest<TModule> where TModule : IModule
    {
    }

    /// <summary>
    /// Manifest for module
    /// </summary>
    public record ModuleManifest : ModuleManifest<IModule>
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; init; } = "";

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; init; } = "";

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; init; } = "";

        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; init; } = "";

        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; init; } = "";

        /// <summary>
        /// Project URL
        /// </summary>
        public string Url { get; init; } = "";

        /// <summary>
        /// Services
        /// </summary>
        public ModuleServiceDescriptor[] Services { get; init; } = Array.Empty<ModuleServiceDescriptor>();

        /// <summary>
        /// Options
        /// </summary>
        public Type[] Options { get; init; } = Array.Empty<Type>();

        /// <summary>
        /// Dependencies
        /// </summary>
        public Type[] Dependencies { get; init; } = Array.Empty<Type>();

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName => $"{Name}@{Version}";
    }
}
