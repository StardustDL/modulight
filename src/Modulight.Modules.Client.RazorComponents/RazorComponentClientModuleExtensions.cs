using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for aspnet modules.
    /// </summary>
    public static class RazorComponentClientModuleExtensions
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

        /// <summary>
        /// Test a type is a page provider type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static bool IsPageProvider(this Type type, Type? providerType = null) => type.IsAssignableTo(providerType ?? typeof(IPageProvider));

        /// <summary>
        /// Test a type is a specified page provider type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPageProvider<T>(this Type type) where T : IPageProvider => type.IsPageProvider(typeof(T));

        /// <summary>
        /// Ensure a type is a page provider type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void EnsurePageProvider(this Type type) => type.EnsurePageProvider<IPageProvider>();

        /// <summary>
        /// Ensure a type is a specified page provider type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public static void EnsurePageProvider<T>(this Type type) where T : IPageProvider
        {
            if (!type.IsPageProvider<T>())
                throw new Exception($"{type.FullName} is not a page provider typed {typeof(T).FullName}.");
        }

        /// <summary>
        /// Get page provider.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IPageProvider? GetPageProvider(this IRazorComponentClientModule module, IModuleHost host)
        {
            var collection = host.GetRazorComponentClientModuleCollection();
            var manifest = collection.GetManifest(module.GetType());
            if (manifest.PageProvider is not null)
            {
                return (IPageProvider)host.Services.GetRequiredService(manifest.PageProvider);
            }
            return null;
        }
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
            if (type.IsAssignableTo(typeof(IComponent)))
            {
                builder.GlobalComponents.Add(type);
            }
            else
            {
                throw new Exception($"The type {type} is not a copmonent.");
            }
            return builder;
        }

        /// <summary>
        /// Add page provider to manifest.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleManifestBuilder WithPageProvider(this IRazorComponentClientModuleManifestBuilder builder, Type type)
        {
            type.EnsurePageProvider();
            builder.PageProvider = type;
            return builder;
        }

        /// <summary>
        /// Add page provider to manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleManifestBuilder WithPageProvider<T>(this IRazorComponentClientModuleManifestBuilder builder) where T : IPageProvider => builder.WithPageProvider(typeof(T));

        /// <summary>
        /// Add global component to manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleManifestBuilder WithGlobalComponent<T>(this IRazorComponentClientModuleManifestBuilder builder) where T : IComponent => builder.WithGlobalComponent(typeof(T));

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
            {
                var attr = type.GetCustomAttribute<ModulePageProviderAttribute>();
                if (attr is not null)
                {
                    builder.WithPageProvider(attr.Type);
                }
            }
            return builder;
        }
    }
}
