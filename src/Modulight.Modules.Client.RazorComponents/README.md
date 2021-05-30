# Modulight.Modules.Client.RazorComponents

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
