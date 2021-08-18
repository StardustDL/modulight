using Modulight.Modules.Client.RazorComponents.UI;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies <see cref="IPageProvider.RootPath"/> for the razor component module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModulePageRootPathAttribute : Attribute
    {
        /// <summary>
        /// Specifies <see cref="IPageProvider.RootPath"/> for the razor component module.
        /// </summary>
        public ModulePageRootPathAttribute(string rootPath)
        {
            RootPath = rootPath;
        }

        /// <summary>
        /// Root path.
        /// </summary>
        public string RootPath { get; }
    }

    /// <summary>
    /// Specifies <see cref="RazorComponentClientModuleManifest.PageProvider"/> for the razor component module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModulePageProviderAttribute : Attribute
    {
        /// <summary>
        /// Specifies <see cref="RazorComponentClientModuleManifest.PageProvider"/> for the razor component module.
        /// </summary>
        public ModulePageProviderAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Page provider.
        /// </summary>
        public Type Type { get; }
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
        /// Global component.
        /// </summary>
        public Type Type { get; }
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
        public UIResourceType Type { get; }

        /// <summary>
        /// Resource relative path.
        /// </summary>
        public string Path { get; } = string.Empty;

        /// <summary>
        /// Attributes for the resource.
        /// </summary>
        public string[] Attributes { get; init; } = Array.Empty<string>();
    }
}
