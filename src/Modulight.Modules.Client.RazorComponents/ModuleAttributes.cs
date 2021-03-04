using Modulight.Modules.Client.RazorComponents.UI;
using System;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies <see cref="IRazorComponentClientModule.RootPath"/> for the razor component module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleUIRootPathAttribute : Attribute
    {
        /// <summary>
        /// Specifies <see cref="IRazorComponentClientModule.RootPath"/> for the razor component module.
        /// </summary>
        public ModuleUIRootPathAttribute(string rootPath)
        {
            RootPath = rootPath;
        }

        /// <summary>
        /// Root path.
        /// </summary>
        public string RootPath { get; init; }
    }

    /// <summary>
    /// Specifies <see cref="RazorComponentClientModuleManifest.GlobalComponents"/> for the razor component module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleUIGlobalComponentAttribute : Attribute
    {
        /// <summary>
        /// Specifies <see cref="RazorComponentClientModuleManifest.GlobalComponents"/> for the razor component module.
        /// </summary>
        public ModuleUIGlobalComponentAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Root path.
        /// </summary>
        public Type Type { get; init; }
    }

    /// <summary>
    /// Specifies UI resources.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleUIResourceAttribute : Attribute
    {
        /// <summary>
        /// Specifies UI resources.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        public ModuleUIResourceAttribute(UIResourceType type, string path)
        {
            Type = type;
            Path = path;
        }

        /// <summary>
        /// Resource type.
        /// </summary>
        public UIResourceType Type { get; init; }

        /// <summary>
        /// Resource relative path.
        /// </summary>
        public string Path { get; init; } = string.Empty;

        /// <summary>
        /// Attributes for the resource.
        /// </summary>
        public string[] Attributes { get; init; } = Array.Empty<string>();
    }
}
