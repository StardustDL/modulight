using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using System;
using System.Reflection;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for aspnet modules.
    /// </summary>
    public static class GraphQLServerModuleExtensions
    {
        /// <summary>
        /// Use building plugin for razor component modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseRazorComponentClientModules(this IModuleHostBuilder modules)
        {
            return modules.ConfigureBuilderServices(services =>
            {
                services.TryAddTransient<IRazorComponentClientModuleManifestBuilder, DefaultRazorComponentClientModuleManifestBuilder>();
            }).UsePlugin<RazorComponentClientModulePlugin>();
        }

        /// <summary>
        /// Get razor component module host from service provider.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleCollection GetRazorComponentClientModuleCollection(this IModuleHost host) => host.Services.GetRequiredService<IRazorComponentClientModuleCollection>();
    }
}

namespace Modulight.Modules
{
    /// <summary>
    /// Extension methods for razor component modules.
    /// </summary>
    public static class RazorComponentClientModuleExtensions
    {
        /// <summary>
        /// Add resource to manifest.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleManifestBuilder WithResource(this IRazorComponentClientModuleManifestBuilder builder, UIResource resource)
        {
            builder.Resources.Add(resource);
            return builder;
        }

        /// <summary>
        /// Add global component to manifest.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleManifestBuilder WithGlobalComponent(this IRazorComponentClientModuleManifestBuilder builder, Type type)
        {
            builder.GlobalComponents.Add(type);
            return builder;
        }

        /// <summary>
        /// Add global component to manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleManifestBuilder WithGlobalComponent<T>(this IRazorComponentClientModuleManifestBuilder builder) where T : IComponent
        {
            builder.GlobalComponents.Add(typeof(T));
            return builder;
        }

        /// <summary>
        /// Configure the builder by default from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleManifestBuilder WithDefaultsFromModuleType(this IRazorComponentClientModuleManifestBuilder builder, Type type)
        {
            {
                var attrs = type.GetCustomAttributes<ModuleUIResourceAttribute>();
                foreach (var attr in attrs)
                {
                    builder.WithResource(new UIResource(attr.Type, attr.Path) { Attributes = attr.Attributes });
                }
            }
            {
                var attrs = type.GetCustomAttributes<ModuleUIGlobalComponentAttribute>();
                foreach (var attr in attrs)
                {
                    builder.WithGlobalComponent(attr.Type);
                }
            }
            return builder;
        }
    }
}
