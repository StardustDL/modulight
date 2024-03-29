﻿using Modulight.Modules.Client.RazorComponents.UI;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Manifest for <see cref="RazorComponentClientModule"/>.
    /// </summary>
    public record RazorComponentClientModuleManifest : ModuleManifest<IRazorComponentClientModule>
    {
        /// <summary>
        /// Get module UI resources.
        /// </summary>
        public UIResource[] Resources { get; init; } = Array.Empty<UIResource>();

        /// <summary>
        /// Get global components.
        /// </summary>
        public Type[] GlobalComponents { get; init; } = Array.Empty<Type>();

        /// <summary>
        /// Get page provider.
        /// </summary>
        public Type? PageProvider { get; init; }
    }
}
