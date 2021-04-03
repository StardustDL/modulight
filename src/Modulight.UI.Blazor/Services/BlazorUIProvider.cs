using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor.Components;
using Modulight.UI.Blazor.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Modulight.UI.Blazor.Services
{
    /// <summary>
    /// Site information.
    /// </summary>
    public record SiteInfo
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; init; } = "Site";

        /// <summary>
        /// Onwer
        /// </summary>
        public string Onwer { get; init; } = "Onwer";

        /// <summary>
        /// Start time (creation time).
        /// </summary>
        public DateTimeOffset StartTime { get; init; }
    }

    /// <summary>
    /// Specifies UI provider for UI
    /// </summary>
    public interface IBlazorUIProvider
    {
        /// <summary>
        /// Information for the site.
        /// </summary>
        SiteInfo SiteInfo { get; }

        /// <summary>
        /// Default module icon for fallback.
        /// </summary>
        RenderFragment? DefaultModuleIcon { get; }

        /// <summary>
        /// Footer
        /// </summary>
        RenderFragment? Footer { get; }

        /// <summary>
        /// View for not fount route.
        /// </summary>
        RenderFragment? RouterNotFound { get; }

        /// <summary>
        /// View for navigating.
        /// </summary>
        RenderFragment? RouterNavigating { get; }

        /// <summary>
        /// Get visible client modules in nav bar.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IRazorComponentClientModule> GetVisibleClientModules();

        /// <summary>
        /// Layout type.
        /// </summary>
        Type DefaultLayout { get; }

        /// <summary>
        /// Root component.
        /// </summary>
        Type RootComponent { get; }

        /// <summary>
        /// Default assembly.
        /// </summary>
        Assembly AppAssembly { get; }

        /// <summary>
        /// Additional assemblies
        /// </summary>
        IEnumerable<Assembly> AdditionalAssemblies { get; }

        /// <summary>
        /// On router navigating
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnNavigateAsync(NavigationContext context);
    }

    /// <summary>
    /// Default implement for <see cref="IBlazorUIProvider"/>.
    /// </summary>
    public class BlazorUIProvider : IBlazorUIProvider
    {
        /// <inheritdoc/>
        public BlazorUIProvider(IModuleHost host)
        {
            Host = host;
            RazorComponentClientModuleCollection = host.GetRazorComponentClientModuleCollection();
            SiteInfo = new SiteInfo
            {
                Name = AppAssembly.GetName().Name ?? "Site",
                StartTime = DateTimeOffset.Now
            };
        }

        /// <inheritdoc/>
        public IModuleHost Host { get; }

        /// <inheritdoc/>
        protected IRazorComponentClientModuleCollection RazorComponentClientModuleCollection { get; }

        /// <inheritdoc/>
        public virtual SiteInfo SiteInfo { get; protected set; }

        /// <inheritdoc/>
        public virtual RenderFragment? DefaultModuleIcon => Fragments.DefaultModuleIcon;

        /// <inheritdoc/>
        public virtual RenderFragment? Footer => Fragments.DefaultFooter(this);

        /// <inheritdoc/>
        public virtual RenderFragment? RouterNotFound => Fragments.DefaultRouterNotFound;

        /// <inheritdoc/>
        public virtual RenderFragment? RouterNavigating => Fragments.DefaultRouterNavigating;

        /// <inheritdoc/>
        public virtual Type DefaultLayout => typeof(ModulePageLayout);

        /// <inheritdoc/>
        public virtual Assembly AppAssembly => GetType().Assembly;

        /// <inheritdoc/>
        public virtual Type RootComponent => typeof(App);

        /// <inheritdoc/>
        public virtual IEnumerable<Assembly> AdditionalAssemblies { get; protected set; } = Array.Empty<Assembly>();

        /// <inheritdoc/>
        public virtual IEnumerable<IRazorComponentClientModule> GetVisibleClientModules()
        {
            return RazorComponentClientModuleCollection.LoadedModules.Where(x =>
            {
                var provider = x.GetPageProvider(Host);
                return provider is not null && provider.RootPath is not "";
            });
        }

        /// <inheritdoc/>
        public virtual async Task OnNavigateAsync(NavigationContext context)
        {
            var results = await RazorComponentClientModuleCollection.GetAssembliesForRouting(context.Path, cancellationToken: context.CancellationToken);
            var filterdResults = new HashSet<Assembly>(results);
            AdditionalAssemblies = filterdResults.Where(x => x != AppAssembly);
        }
    }
}
