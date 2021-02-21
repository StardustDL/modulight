using System;

namespace Modulight.Modules.Client.RazorComponents.UI
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
}
