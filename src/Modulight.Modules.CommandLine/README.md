# Modulight.Modules.CommandLine

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