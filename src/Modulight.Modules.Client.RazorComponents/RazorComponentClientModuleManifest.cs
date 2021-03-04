using Modulight.Modules.Client.RazorComponents.UI;
using System;
using System.Linq;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Manifest for <see cref="RazorComponentClientModule"/>.
    /// </summary>
    public record RazorComponentClientModuleManifest
    {
        /// <summary>
        /// Get module UI resources.
        /// </summary>
        public UIResource[] Resources { get; init; } = Array.Empty<UIResource>();

        /// <summary>
        /// Get global components.
        /// </summary>
        public Type[] GlobalComponents { get; init; } = Array.Empty<Type>();
    }
}
