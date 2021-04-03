using Delights.Modules.Hello.GraphQL;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using StardustDL.RazorComponents.MaterialDesignIcons;
using System;

namespace Delights.Modules.Hello
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddHelloModule(this IModuleHostBuilder builder, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<HelloModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureOptions(configureOptions);
            }
            return builder;
        }
    }

    [Module(Url = SharedManifest.Url, Author = SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleStartup(typeof(Startup))]
    [ModulePageProvider(typeof(MPageProvider))]
    [ModuleService(typeof(ModuleService))]
    [ModuleUIResource(UIResourceType.Assembly, "Delights.Modules.Hello.UI")]
    [ModuleDependency(typeof(MaterialDesignIconModule))]
    public class HelloModule : RazorComponentClientModule
    {
        public HelloModule(IModuleHost host) : base(host)
        {
        }

        public override RenderFragment Icon => Fragments.Icon;
    }

    [ModulePageRootPath("hello")]
    class MPageProvider : PageProvider
    {
        
    }

    class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHelloClient().ConfigureHttpClient((sp, client) =>
            {
                var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/Hello");
            });
            base.ConfigureServices(services);
        }
    }

    public class ModuleService
    {
        public HelloClient GraphQLClient { get; }

        public ModuleService(HelloClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
