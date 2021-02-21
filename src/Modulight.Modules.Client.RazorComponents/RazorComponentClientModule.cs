using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies the contract for razor component modules.
    /// </summary>
    public interface IRazorComponentClientModule : IModule
    {
        /// <summary>
        /// Get module icon.
        /// </summary>
        RenderFragment? Icon { get; }

        /// <summary>
        /// Get module UI resources.
        /// </summary>
        UIResource[] Resources { get; }

        /// <summary>
        /// Get global components.
        /// </summary>
        Type[] GlobalComponents { get; }

        /// <summary>
        /// Get module UI route root path, such as home, search, and so on.
        /// Use <see cref="string.Empty"/> for no page module.
        /// </summary>
        string RootPath { get; }

        /// <summary>
        /// Check if a path is belongs to this module UI.
        /// </summary>
        /// <param name="path">Route path.</param>
        /// <returns></returns>
        bool Contains(string path);
    }

    /// <summary>
    /// Basic implement for <see cref="IRazorComponentClientModule"/>.
    /// </summary>
    [ModuleDependency(typeof(RazorComponentClientCoreModule))]
    public abstract class RazorComponentClientModule : Module, IRazorComponentClientModule
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected RazorComponentClientModule(IModuleHost host) : base(host)
        {
            RootPath = "";
            var type = GetType();
            {
                var attr = type.GetCustomAttribute<ModuleUIRootPathAttribute>();
                if (attr is not null)
                    RootPath = attr.RootPath;
            }
            {
                var attrs = type.GetCustomAttributes<ModuleUIResourceAttribute>();
                List<UIResource> resources = new List<UIResource>();
                foreach (var attr in attrs)
                {
                    resources.Add(new UIResource(attr.Type, attr.Path) { Attributes = attr.Attributes });
                }
                Resources = resources.ToArray();
            }
            {
                var attrs = type.GetCustomAttributes<ModuleUIGlobalComponentAttribute>();
                List<Type> resources = new List<Type>();
                foreach (var attr in attrs)
                {
                    resources.Add(attr.Type);
                }
                GlobalComponents = resources.ToArray();
            }
        }

        /// <inheritdoc/>
        public virtual RenderFragment? Icon => null;

        /// <inheritdoc/>
        public string RootPath { get; }

        /// <inheritdoc/>
        public virtual bool Contains(string path)
        {
            if (RootPath is "")
            {
                return true;
            }
            path = path.Trim('/') + "/";
            return path.StartsWith($"{RootPath}/");
        }

        /// <inheritdoc/>
        public UIResource[] Resources { get; protected set; }

        /// <inheritdoc/>
        public Type[] GlobalComponents { get; protected set; }
    }

    [Module(Author = "StardustDL", Description = "Provide services for razor component client modules.", Url = "https://github.com/StardustDL/delights")]
    [ModuleService(typeof(RazorComponentClientModuleCollection), ServiceType = typeof(IRazorComponentClientModuleCollection), Lifetime = ServiceLifetime.Singleton)]
    [ModuleService(typeof(JSModuleProvider<>), ServiceType = typeof(IJSModuleProvider<>))]
    [ModuleService(typeof(LazyAssemblyLoader))]
    [ModuleService(typeof(ModuleUILoader))]
    class RazorComponentClientCoreModule : Module
    {
        public RazorComponentClientCoreModule(IModuleHost host, IRazorComponentClientModuleCollection collection) : base(host)
        {
            Collection = collection;
        }

        public IRazorComponentClientModuleCollection Collection { get; }

        public override async Task Initialize()
        {
            if (Environment.OSVersion.Platform is PlatformID.Other)
            {
                await Collection.LoadResources();
            }
            await base.Initialize();
        }
    }
}
