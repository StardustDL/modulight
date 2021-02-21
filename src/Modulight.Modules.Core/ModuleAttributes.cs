using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules
{
    /// <summary>
    /// Specifies the manifest for the module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleAttribute : Attribute
    {
        /// <summary>
        /// Set module manifest.
        /// </summary>
        public ModuleAttribute() : this(null) { }

        /// <summary>
        /// Set module manifest.
        /// </summary>
        public ModuleAttribute(string? name = null)
        {
            Name = name;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Display name
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Version
        /// </summary>
        public string? Version { get; init; }

        /// <summary>
        /// Author
        /// </summary>
        public string? Author { get; init; }

        /// <summary>
        /// Project URL
        /// </summary>
        public string? Url { get; init; }
    }

    /// <summary>
    /// Specifies the service for the module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleServiceAttribute : Attribute
    {
        /// <summary>
        /// Specifies the service for the module.
        /// </summary>
        /// <param name="implementationType">The implementation type.</param>
        public ModuleServiceAttribute(Type implementationType)
        {
            ImplementationType = implementationType;
        }

        /// <summary>
        /// The service type. Null to use the implement type.
        /// </summary>
        public Type? ServiceType { get; init; }

        /// <summary>
        /// The implementation type.
        /// </summary>
        public Type ImplementationType { get; init; }

        /// <summary>
        /// Service lifetime (default as <see cref="ServiceLifetime.Scoped"/>).
        /// </summary>
        public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Scoped;

        /// <summary>
        /// Behavior when the service been added.
        /// </summary>
        public ServiceRegisterBehavior RegisterBehavior { get; init; }
    }

    /// <summary>
    /// Behavior when the service been added.
    /// </summary>
    public enum ServiceRegisterBehavior
    {
        /// <summary>
        /// Normal (Use Add).
        /// </summary>
        Normal,
        /// <summary>
        /// The service is optional (Use TryAdd).
        /// </summary>
        Optional,
    }

    /// <summary>
    /// Specifies the option for the module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleOptionAttribute : Attribute
    {
        /// <summary>
        /// Specifies the option for the module.
        /// </summary>
        /// <param name="optionType">Type for the option.</param>
        public ModuleOptionAttribute(Type optionType)
        {
            OptionType = optionType;
        }

        /// <summary>
        /// Type for the option.
        /// </summary>
        public Type OptionType { get; init; }
    }

    /// <summary>
    /// Specifies the startup for the module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleStartupAttribute : Attribute
    {
        /// <summary>
        /// Specifies the startup for the module.
        /// </summary>
        /// <param name="startupType">The startup type. It must implement <see cref="IModuleStartup"/>.</param>
        public ModuleStartupAttribute(Type startupType)
        {
            StartupType = startupType;
        }

        /// <summary>
        /// The startup type. It must implement <see cref="IModuleStartup"/>.
        /// </summary>
        public Type StartupType { get; init; }
    }

    /// <summary>
    /// Specifies the dependency for the module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ModuleDependencyAttribute : Attribute
    {
        /// <summary>
        /// Specifies the dependency for the module.
        /// </summary>
        /// <param name="moduleType">The dependency module type. It must implement <see cref="IModule"/>.</param>
        public ModuleDependencyAttribute(Type moduleType)
        {
            ModuleType = moduleType;
        }

        /// <summary>
        /// The dependency module type. It must implement <see cref="IModule"/>.
        /// </summary>
        public Type ModuleType { get; init; }
    }
}
