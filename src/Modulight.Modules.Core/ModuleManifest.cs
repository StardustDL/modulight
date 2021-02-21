using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modulight.Modules
{
    /// <summary>
    /// Service descriptor for module services.
    /// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public record ModuleServiceDescriptor(Type ImplementationType, Type ServiceType, ServiceLifetime Lifetime = ServiceLifetime.Scoped, ServiceRegisterBehavior RegisterBehavior = ServiceRegisterBehavior.Normal)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
    }

    /// <summary>
    /// Manifest for module
    /// </summary>
    public record ModuleManifest
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
    }
}
