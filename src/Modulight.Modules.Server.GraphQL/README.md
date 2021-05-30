# Modulight.Modules.Server.GraphQL

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