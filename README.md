# Modulight

![](https://github.com/StardustDL/modulight/workflows/CI/badge.svg) ![](https://img.shields.io/github/license/StardustDL/modulight.svg) [![](https://buildstats.info/nuget/Modulight.Modules.Core)](https://www.nuget.org/packages/Modulight.Modules.Core/)

[Modulight](https://github.com/StardustDL/modulight) is a light modular framework aimed to be low intrusive based on dependency injection for .NET 5, ASP.NET Core and Blazor.

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