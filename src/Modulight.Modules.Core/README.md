# Modulight.Modules.Core

![](https://github.com/StardustDL/modulight/workflows/CI/badge.svg) ![](https://img.shields.io/github/license/StardustDL/modulight.svg) [![](https://buildstats.info/nuget/Modulight.Modules.Core)](https://www.nuget.org/packages/Modulight.Modules.Core/)

[Modulight](https://github.com/StardustDL/modulight) is a light modular framework aimed to be low intrusive based on dependency injection for .NET, ASP.NET, Blazor, and command-line.

## Use modules

1. Register modules.

For general modules:

```csharp
services.AddModules(builder => {
    builder.AddModule<FooModule>();
});
```

2. Configure the module initilizing & shutdown.

```csharp
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

```csharp
// Generic hosting. (provided by package Modulight.Modules.Hosting)
// in Program: Task Main(string[] args)
await CreateHostBuilder(args).Build().RunAsyncWithModules();

// WebAssembly hosting. (provided by package Modulight.Modules.Client.RazorComponents)
// in Program: Task Main(string[] args)
await builder.Build().RunAsyncWithModules();
```
