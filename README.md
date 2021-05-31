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

See [here](./src/Modulight.Modules.Core/README.md) for details.

### Addition steps

#### Use Command line modules

See [here](./src/Modulight.Modules.CommandLine/README.md) for details.

#### Use Razor component modules

See [here](./src/Modulight.Modules.Client.RazorComponents/README.md) for details.

#### Use Blazor UI hosting template

See [here](./src/Modulight.UI.Blazor/README.md) for details.

#### Use ASP.NET modules

See [here](./src/Modulight.Modules.Server.AspNet/README.md) for details.

#### Use GraphQL modules

See [here](./src/Modulight.Modules.Server.GraphQL/README.md) for details.

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