# Modulight.Modules.Server.AspNet

[![](https://buildstats.info/nuget/Modulight.Modules.Server.AspNet)](https://www.nuget.org/packages/Modulight.Modules.Server.AspNet/)

```csharp
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
