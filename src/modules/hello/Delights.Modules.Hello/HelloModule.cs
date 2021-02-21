using Delights.Modules.Hello.GraphQL;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using StardustDL.RazorComponents.MaterialDesignIcons;
using System;
using System.Threading.Tasks;

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
    [ModuleService(typeof(ModuleService))]
    [ModuleUIRootPath("hello")]
    [ModuleUIResource(UIResourceType.Assembly, "Delights.Modules.Hello.UI")]
    [ModuleDependency(typeof(MaterialDesignIconModule))]
    public class HelloModule : RazorComponentClientModule
    {
        public HelloModule(IModuleHost host) : base(host)
        {
        }

        public override RenderFragment Icon => Fragments.Icon;
    }

    class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(
                "HelloGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/Hello");
                });
            services.AddHelloGraphQLClient();
            base.ConfigureServices(services);
        }
    }

    public class ModuleService
    {
        public IHelloGraphQLClient GraphQLClient { get; }

        public ModuleService(IHelloGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
        }
    }
}
