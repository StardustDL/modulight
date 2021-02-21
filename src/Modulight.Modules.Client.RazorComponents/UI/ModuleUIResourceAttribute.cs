using System;

namespace Modulight.Modules.Client.RazorComponents.UI
{
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
