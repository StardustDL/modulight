using System;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Data for a module in host builder.
    /// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public record ModuleDefinition(Type Type, ModuleManifest Manifest, IModuleStartup? Startup);
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
}