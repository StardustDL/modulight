using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules
{
    /// <summary>
    /// Specifies the interface to build a module manifest.
    /// </summary>
    public interface IModuleManifestBuilder
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// Project URL
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Services
        /// </summary>
        IList<ModuleServiceDescriptor> Services { get; }

        /// <summary>
        /// Options
        /// </summary>
        IList<Type> Options { get; }

        /// <summary>
        /// Dependencies
        /// </summary>
        IList<Type> Dependencies { get; }

        /// <summary>
        /// Build the manifest.
        /// </summary>
        /// <returns></returns>
        ModuleManifest Build();
    }

    class DefaultModuleManifestBuilder : IModuleManifestBuilder
    {
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Version { get; set; } = "";
        public string Author { get; set; } = "";
        public string Url { get; set; } = "";

        public IList<ModuleServiceDescriptor> Services { get; } = new List<ModuleServiceDescriptor>();

        public IList<Type> Options { get; } = new List<Type>();

        public IList<Type> Dependencies { get; } = new List<Type>();

        public ModuleManifest Build()
        {
            return new ModuleManifest
            {
                Author = Author,
                Dependencies = Dependencies.ToArray(),
                Description = Description,
                DisplayName = DisplayName,
                Name = Name,
                Options = Options.ToArray(),
                Services = Services.ToArray(),
                Url = Url,
                Version = Version,
            };
        }
    }
}
