using System;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// Specifies <see cref="IRazorComponentClientModule.GlobalComponents"/> for the razor component module.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleUIGlobalComponentAttribute : Attribute
    {
        /// <summary>
        /// Specifies <see cref="IRazorComponentClientModule.GlobalComponents"/> for the razor component module.
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
}
