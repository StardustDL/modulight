using Delights.Modules.Hello;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Hosting;
using Modulight.UI.Blazor;
using Modulight.UI.Blazor.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Modulights.UI
{
    public class TestBlazorUIProvider : BlazorUIProvider
    {
        public TestBlazorUIProvider(IRazorComponentClientModuleCollection razorComponentClientModuleCollection) : base(razorComponentClientModuleCollection)
        {
        }
    }
}

namespace Test.Modulights.UI.Wasm
{
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
                    .AddHelloModule((o, _) => o.GraphQLEndpoint = "https://localhost:5001");
            });

            var host = builder.Build();

            await using var _ = await host.Services.UseModuleHost();

            await host.RunAsync();
        }
    }
}
