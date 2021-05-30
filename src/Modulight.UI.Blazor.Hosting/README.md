# Modulight.UI.Blazor.Hosting

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
