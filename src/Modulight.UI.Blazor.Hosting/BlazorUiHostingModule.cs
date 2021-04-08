using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.AspNet;

namespace Modulight.UI.Blazor.Hosting
{
    /// <summary>
    /// Blazor UI Hosting Module
    /// </summary>
    [ModuleStartup(typeof(Startup))]
    [Module(Url = "https://github.com/StardustDL/modulight", Author = "StardustDL", Description = "Provide hosting services and prerendering for blazor client module hosting.")]
    [ModuleDependency(typeof(BlazorUiModule))]
    [ModuleOption(typeof(BlazorUiHostingModuleOption))]
    public class BlazorUiHostingModule : AspNetServerModule
    {
        /// <summary>
        /// Create instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="options"></param>
        public BlazorUiHostingModule(IModuleHost host, IOptions<BlazorUiHostingModuleOption> options) : base(host)
        {
            Options = options.Value;
        }

        /// <summary>
        /// Options
        /// </summary>
        public BlazorUiHostingModuleOption Options { get; }

        /// <inheritdoc/>
        public override void UseMiddleware(IApplicationBuilder builder)
        {
            if (Options.HostingModel is HostingModel.Client && Options.DefaultBlazorFrameworkFiles)
            {
                builder.UseBlazorFrameworkFiles();
            }
            base.UseMiddleware(builder);
        }

        /// <inheritdoc/>
        public override void MapEndpoint(IEndpointRouteBuilder builder)
        {
            if (Options.HostingModel is HostingModel.Server && Options.DefaultBlazorHub)
            {
                builder.MapBlazorHub();
            }
            builder.MapFallbackToAreaPage("/_Host", "Modulights");
            base.MapEndpoint(builder);
        }
    }

    class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
            base.ConfigureServices(services);
        }
    }
}
