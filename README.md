# Modulight

![](https://github.com/StardustDL/modulight/workflows/CI/badge.svg) ![](https://img.shields.io/github/license/StardustDL/modulight.svg) [![](https://buildstats.info/nuget/Modulight.Modules.Core)](https://www.nuget.org/packages/Modulight.Modules.Core/)

[Modulight](https://github.com/StardustDL/modulight) is a light modular framework aimed to be low intrusive based on dependency injection for .NET 5, ASP.NET Core and Blazor.

## Features

- Dependency injection
- Unified services registering
- CommandLine (cooperated with [CliFx](https://github.com/Tyrrrz/CliFx))
  - Hosting in generic host Microsoft.Extensions.Hosting.IHost
  - Dependency injection, logging & hosting services
  - Extensible & Composable
- Blazor Client & Server
  - Unified CSS & JS lazy loading & prerendering. No need to append `<script>` and `<link>` repeatedly for every razor components, especially when use different hosting models.
  - Unified assembly lazy loading.
  - Interop between modules and host.
  - A builtin hosting implementation with prerendering.
- ASP.NET Server
  - Custom middlewares
  - Custom endpoints
- GraphQL Server (cooperated with [ChilliCream GraphQL Platform](https://github.com/ChilliCream/hotchocolate))
  - Unified query/mutation/subscription definition
- Builtin module dependencies, options & services support

## Usage

### Use modules

1. Register modules.

For general modules:

```cs
services.AddModules(builder => {
    builder.AddModule<FooModule>();
});
```

2. Configure the module initilizing & shutdown.

```cs
var host = services.GetModuleHost();
await host.Initialize();

// do something

await host.Shutdown();

// Or use context:

// context: IServiceProvider services (provided by package Modulight.Modules.Core)
await using var _ = await services.UseModuleHost();

// do something
```

Or use extension methods for hosting:

```cs
// ASP.NET hosting. (provided by package Modulight.Modules.Server.AspNet)
// in Program: Task Main(string[] args)
await CreateHostBuilder(args).Build().RunAsyncWithModules();

// WebAssembly hosting. (provided by package Modulight.Modules.Client.RazorComponents)
// in Program: Task Main(string[] args)
await builder.Build().RunAsyncWithModules();
```

### Addition steps

#### Use Command line modules

[![](https://buildstats.info/nuget/Modulight.Modules.CommandLine)](https://www.nuget.org/packages/Modulight.Modules.CommandLine/)

```cs
class Program
{
    public static async Task Main(string[] args) => await CreateHostBuilder(args).RunConsoleAsyncWithModules();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddModules(builder =>
                {
                    builder.UseCommandLineModules().AddModule<AModule>();
                });
            });
}

[Command]
public class LogCommand : ModuleCommand<AModule>
{
    public LogCommand(AModule module) : base(module)
    {
    }

    // Order: 0
    [CommandParameter(0, Description = "Value whose logarithm is to be found.")]
    public double Value { get; init; }

    // Name: --base
    // Short name: -b
    [CommandOption("base", 'b', Description = "Logarithm base.")]
    public double Base { get; init; } = 10;

    protected override ValueTask ExecuteAsync(IConsole console, CancellationToken cancellationToken = default)
    {
        var result = Math.Log(Value, Base);
        console.Output.WriteLine(result);
        console.Output.WriteLine($"From Module {Module.Manifest.DisplayName}.");

        return default;
    }
}

[CommandFrom(typeof(LogCommand))]
public class AModule : CommandLineModule
{
    public AModule(IModuleHost host) : base(host)
    {
    }
}
```

A [Sample startup](https://github.com/StardustDL/modulight/blob/master/test/Test.CommandLine/Program.cs).

#### Use Razor component modules

[![](https://buildstats.info/nuget/Modulight.Modules.Client.RazorComponents)](https://www.nuget.org/packages/Modulight.Modules.Client.RazorComponents/)

Modulight provides a place to unify resources, and it can be used to make Razor component library easy to use and manage. The user needn't to take care of related services and `<script>` or `<link>` tags in `index.html`.

```cs
// in Startup: void ConfigureServices(ISeviceCollection services)

services.AddModules(builder => {
    builder.UseRazorComponentClientModules().AddModule<FooModule>();
});
```

For razor components, add `ResourceDeclare` component to App.razor to load UI resources.

```razor
<Modulight.Modules.Client.RazorComponents.UI.ResourceDeclare />
```

This component will find all resources and global components defined in modules, and render HTML tags for them.

You can also use the following codes to load resources manually (needs IJSRuntime).

```cs
// WebAssemblyHost host;
await host.Services.GetRazorComponentClientModuleCollection().LoadResources();
```

### Use Blazor UI hosting template

Modulight provide a template project for Blazor hosting with Razor Component Client modules.

Use the package [Modulight.UI.Blazor ![](https://buildstats.info/nuget/Modulight.UI.Blazor?includePreReleases=true)](https://www.nuget.org/packages/Modulight.UI.Blazor/) to try it.

It provides a navigation layout generated by client modules.

First implement a custom Blazor UI provider.

```cs
class CustomBlazorUIProvider : BlazorUIProvider
{
    public CustomBlazorUIProvider(IRazorComponentClientModuleCollection razorComponentClientModuleCollection) : base(razorComponentClientModuleCollection)
    {
    }
}
```

Then use the provider.

#### In WebAssembly

```cs
builder.Services.AddModules(builder =>
{
    builder.UseRazorComponentClientModules().AddBlazorUI<CustomBlazorUIProvider>();
});
```

A [Sample startup](https://github.com/StardustDL/modulight/blob/master/test/Test.Modulights.UI.Wasm/Program.cs).

#### In ASP.NET hosting

It needs the package [Modulight.UI.Blazor.Hosting ![](https://buildstats.info/nuget/Modulight.UI.Blazor.Hosting?includePreReleases=true)](https://www.nuget.org/packages/Modulight.UI.Blazor.Hosting/) to support prerendering.

```cs
// void ConfigureServices(IServiceCollection services)

services.AddModules(builder =>
{
    builder.UseRazorComponentClientModules().AddServerSideBlazorUI<CustomBlazorUIProvider>();
    // builder.UseRazorComponentClientModules().AddClientSideBlazorUI<CustomBlazorUIProvider>();
});

// void Configure(IApplicationBuilder app, IWebHostEnvironment env)

app.UseAspNetServerModuleMiddlewares();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapAspNetServerModuleEndpoints();
});

// in Program: Task Main(string[] args)
await CreateHostBuilder(args).Build().RunAsyncWithModules();
```

A [Sample startup](https://github.com/StardustDL/modulight/blob/master/test/Test.Modulights.UI/Startup.cs).

#### Use ASP.NET modules

[![](https://buildstats.info/nuget/Modulight.Modules.Server.AspNet)](https://www.nuget.org/packages/Modulight.Modules.Server.AspNet/)

```cs
// in Startup: void ConfigureServices(ISeviceCollection services)

services.AddModules(builder => {
    builder.UseAspNetServerModules().AddModule<FooModule>();
});

// in Startup: void Configure(IApplicationBuilder app, IWebHostEnvironment env)

app.UseAspNetServerModuleMiddlewares();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAspNetServerModuleEndpoints();
});
```

#### Use GraphQL modules

[![](https://buildstats.info/nuget/Modulight.Modules.Server.GraphQL)](https://www.nuget.org/packages/Modulight.Modules.Server.GraphQL/)

```cs
// in Startup: void ConfigureServices(ISeviceCollection services)

services.AddModules(builder => {
    builder.UseGraphQLServerModules().AddModule<FooModule>();
});

// in Startup: void Configure(IApplicationBuilder app, IWebHostEnvironment env)

app.UseEndpoints(endpoints =>
{
    // modules mapper
    endpoints.MapGraphQLServerModuleEndpoints(postMapEndpoint: (module, builder) =>
    {
        builder.RequireCors(cors =>
        {
            cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    });
});
```

## Example codes

They are based on nightly build package at: 

[NUGET source](https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json)

### Design and use a command line module in console application

- [Program.cs](https://github.com/StardustDL/modulight/blob/master/test/Test.CommandLine/Program.cs)

### Design a client (Blazor) module

- [HelloModule.cs](https://github.com/StardustDL/modulight/blob/master/src/modules/hello/Delights.Modules.Hello/HelloModule.cs) Client module definition.
- [Index.razor](https://github.com/StardustDL/modulight/blob/master/src/modules/hello/Delights.Modules.Hello.UI/Pages/Index.razor) Client module pages. It belongs to a different assembly from which Module belongs to because we want this assembly is lazy loading.

### Design a GraphQL server module

- [HelloServerModule.cs](https://github.com/StardustDL/modulight/blob/master/src/modules/hello/Delights.Modules.Hello.Server/HelloServerModule.cs) GraphQL server module definition.

### Use a client module in Blazor websites

- [ModulePageLayout.razor](https://github.com/StardustDL/modulight/blob/master/src/Modulight.UI.Blazor/Layouts/ModulePageLayout.razor) Layout and container for module pages.
- [App.razor](https://github.com/StardustDL/modulight/blob/master/src/Modulight.UI.Blazor/App.razor) Lazy loading for js/css/sassemblies when routing.
- [AntDesignModule.cs](https://github.com/StardustDL/razorcomponents/blob/master/src/AntDesigns/AntDesignModule.cs) Definition of JS/CSS resources.
- [ModuleSetup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Client.Shared/ModuleSetup.cs) Use modules in client.
- [Startup.cs](https://github.com/StardustDL/modulight/blob/master/test/Test.Modulights.UI/Startup.cs) Blazor Server hosting.
- [Program.cs](https://github.com/StardustDL/modulight/blob/master/test/Test.Modulights.UI.Wasm/Program.cs) Blazor WebAssembly hosting.
- [index.html](https://github.com/StardustDL/modulight/blob/master/test/Test.Modulights.UI.Wasm/wwwroot/index.html) Clean index.html.

### Use a GraphQL server module

- [Startup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Api/Startup.cs) GraphQL server integrating.

## Project guide

- [Modulight.Modules.Core](./src/Modulight.Modules.Core/) Core types for Modulight framework.
- [Modulight.Modules.CommandLine](./src/Modulight.Modules.CommandLine/) Basic types for command line modules.
- [Modulight.Modules.Client.RazorComponents](./src/Modulight.Modules.Client.RazorComponents/) Basic types for razor component client modules.
- [Modulight.Modules.Server.AspNet](./src/Modulight.Modules.Server.AspNet/) Basic types for aspnet server modules.
- [Modulight.Modules.Server.GraphQL](./src/Modulight.Modules.Server.GraphQL/) Basic types for graphql server modules.
- [Hello module](./src/modules/hello/) A demo module.
  - [Hello](./src/modules/hello/Delights.Modules.Hello) Client module.
  - [Hello.Core](./src/modules/hello/Delights.Modules.Hello.Core) Shared manifest between client & server module.
  - [Hello.UI](./src/modules/hello/Delights.Modules.Hello.UI) UI (pages) for client module.
  - [Hello.Server](./src/modules/hello/Delights.Modules.Hello.Server) Server module.