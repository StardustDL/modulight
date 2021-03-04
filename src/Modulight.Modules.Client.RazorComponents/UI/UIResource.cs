using System;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// UI Resource type.
    /// </summary>
    public enum UIResourceType
    {
        /// <summary>
        /// Javascript
        /// </summary>
        Script,
        /// <summary>
        /// Style sheet
        /// </summary>
        StyleSheet,
        /// <summary>
        /// Assembly
        /// </summary>
        Assembly,
    }

    /// <summary>
    /// Defines UI resourcecs.
    /// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public record UIResource(UIResourceType Type, string Path)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
        /// <summary>
        /// Attributes for the resource.
        /// </summary>
        public string[] Attributes { get; init; } = Array.Empty<string>();
    }
}
