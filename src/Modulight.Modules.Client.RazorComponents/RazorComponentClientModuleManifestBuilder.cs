using Modulight.Modules.Client.RazorComponents.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies the interface to build a module manifest.
    /// </summary>
    public interface IRazorComponentClientModuleManifestBuilder : IModuleManifestBuilder<IRazorComponentClientModule, RazorComponentClientModuleManifest>
    {
        /// <summary>
        /// Get module UI resources.
        /// </summary>
        IList<UIResource> Resources { get; }

        /// <summary>
        /// Get global components.
        /// </summary>
        IList<Type> GlobalComponents { get; }
    }

    class DefaultRazorComponentClientModuleManifestBuilder : IRazorComponentClientModuleManifestBuilder
    {
        public IList<UIResource> Resources { get; } = new List<UIResource>();

        public IList<Type> GlobalComponents { get; } = new List<Type>();

        public RazorComponentClientModuleManifest Build()
        {
            return new RazorComponentClientModuleManifest
            {
                GlobalComponents = GlobalComponents.ToArray(),
                Resources = Resources.ToArray()
            };
        }
    }
}
