# Modulight

![](https://github.com/StardustDL/modulight/workflows/CI/badge.svg) ![](https://img.shields.io/github/license/StardustDL/modulight.svg) [![](https://buildstats.info/nuget/Modulight.Modules.Core)](https://www.nuget.org/packages/Modulight.Modules.Core/)

[Modulight](https://github.com/StardustDL/modulight) is a light modular framework aimed to be low intrusive based on dependency injection for .NET 5, ASP.NET Core and Blazor.

## Features

- Dependency injection
- Unified services registering
- Client (Blazor)
  - Unified CSS & JS lazy loading & prerendering. No need to append `<script>` and `<link>` repeatedly for every razor components, especially when use different hosting models.
  - Unified assembly lazy loading.
  - Interop between modules and host.
- Server (GraphQL cooperated with [ChilliCream GraphQL Platform](https://github.com/ChilliCream/hotchocolate))
  - Unified query/mutation/subscription definition
- Builtin module options support

It provides a place to unify resources, and it can be used to make Razor component library easy to use and manage. The user needn't to take care of related services and `<script>` or `<link>` tags in `index.html`.

## Usage

### Use modules

1. Register modules.

For general modules:

```cs
services.AddModules(builder => {
    builder.AddModule<FooModule>();
});
```

1. Configure the module initilizing & shutdown.

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

### Addition steps

#### Use Razor component modules

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

#### Use ASP.NET modules

```cs
// in Startup: void Configure(IApplicationBuilder app, IWebHostEnvironment env)

app.UseAspNetServerModuleMiddlewares();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAspNetServerModuleEndpoints();
});
```

#### Use GraphQL modules

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

### Hosting

```cs
// ASP.NET hosting. (provided by package Modulight.Modules.Server.AspNet)
// in Program: Task Main(string[] args)
await CreateHostBuilder(args).Build().RunAsyncWithModules();

// WebAssembly hosting. (provided by package Modulight.Modules.Client.RazorComponents)
// in Program: Task Main(string[] args)
await builder.Build().RunAsyncWithModules();
```

### Use Blazor UI Template

Modulight provide a template project for Blazor hosting with Razor Component Client modules.

Use package [Modulight.UI.Blazor ![](https://buildstats.info/nuget/Modulight.UI.Blazor?includePreReleases=true)](https://www.nuget.org/packages/Modulight.UI.Blazor/) and [Modulight.UI.Blazor.Hosting ![](https://buildstats.info/nuget/Modulight.UI.Blazor.Hosting?includePreReleases=true)](https://www.nuget.org/packages/Modulight.UI.Blazor.Hosting/) to try it.

It provides a navigation layout generated by client modules, and supports prerendering.

```cs
class CustomBlazorUIProvider : BlazorUIProvider
{
    public CustomBlazorUIProvider(IRazorComponentClientModuleCollection razorComponentClientModuleCollection) : base(razorComponentClientModuleCollection)
    {
    }
}

// void ConfigureServices(IServiceCollection services)

services.AddModules(builder =>
{
    builder.AddServerSideBlazorUI<CustomBlazorUIProvider>();
});

// void Configure(IApplicationBuilder app, IWebHostEnvironment env)

app.UseAspNetServerModuleMiddlewares();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapAspNetServerModuleEndpoints();
});
```

A [Sample startup](https://github.com/StardustDL/modulight/blob/master/test/Test.Modulights.UI/Startup.cs).

## Example codes

They are based on nightly build package at: 

[NUGET source](https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json)

### Design a client (Blazor) module

- [HelloModule.cs](https://github.com/StardustDL/modulight/blob/master/src/modules/hello/Delights.Modules.Hello/HelloModule.cs) Client module definition.
- [Index.razor](https://github.com/StardustDL/modulight/blob/master/src/modules/hello/Delights.Modules.Hello.UI/Pages/Index.razor) Client module pages. It belongs to a different assembly from which Module belongs to because we want this assembly is lazy loading.

### Design a GraphQL server module

- [HelloServerModule.cs](https://github.com/StardustDL/modulight/blob/master/src/modules/hello/Delights.Modules.Hello.Server/HelloServerModule.cs) GraphQL server module definition.

### Use a client module in Blazor websites

- [ModulePageLayout.razor](https://github.com/StardustDL/delights/blob/master/src/Delights.UI/Shared/ModulePageLayout.razor) Layout and container for module pages.
- [App.razor](https://github.com/StardustDL/delights/blob/master/src/Delights.UI/App.razor) Lazy loading for js/css/sassemblies when routing.
- [UIModule.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.UI/UIModule.cs) Definition of JS/CSS resources.
- [ModuleSetup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Client.Shared/ModuleSetup.cs) Use modules in client.
- [Startup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Client/Startup.cs) Blazor Server hosting.
- [Program.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Client.WebAssembly/Program.cs) Blazor WebAssembly hosting.
- [index.html](https://github.com/StardustDL/delights/blob/master/src/Delights.Client.WebAssembly/wwwroot/index.html) Clean index.html.

### Use a GraphQL server module

- [Startup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Api/Startup.cs) GraphQL server integrating.

## Project guide

- [Modulight.Modules.Core](./src/Modulight.Modules.Core/) Core types for Modulight framework.
- [Modulight.Modules.Client.RazorComponents](./src/Modulight.Modules.Client.RazorComponents/) Basic types for razor component client modules.
- [Modulight.Modules.Server.AspNet](./src/Modulight.Modules.Server.AspNet/) Basic types for aspnet server modules.
- [Modulight.Modules.Server.GraphQL](./src/Modulight.Modules.Server.GraphQL/) Basic types for graphql server modules.
- [Hello module](./src/modules/hello/) A demo module.
  - [Hello](./src/modules/hello/Delights.Modules.Hello) Client module.
  - [Hello.Core](./src/modules/hello/Delights.Modules.Hello.Core) Shared manifest between client & server module.
  - [Hello.UI](./src/modules/hello/Delights.Modules.Hello.UI) UI (pages) for client module.
  - [Hello.Server](./src/modules/hello/Delights.Modules.Hello.Server) Server module.