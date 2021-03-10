using Delights.Modules.Hello;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test.Modulights.UI
{
    public class TestBlazorUIProvider : BlazorUIProvider
    {
        public TestBlazorUIProvider(IModuleHost host) : base(host)
        {
        }
    }
}

namespace Test.Modulights.UI.Wasm
{
    public class TestModule : RazorComponentClientModule
    {
        public TestModule(IModuleHost host) : base(host)
        {
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Modulight.UI.Blazor.App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddModules(builder =>
            {
                builder.AddBlazorUI<TestBlazorUIProvider>()
                    .AddModule<TestModule>()
                    .AddHelloModule((o, _) => o.GraphQLEndpoint = "https://localhost:5001/graphql");
            });

            await builder.Build().RunAsyncWithModules();
        }
    }
}
