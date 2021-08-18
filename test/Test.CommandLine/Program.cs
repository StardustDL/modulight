using CliFx.Attributes;
using CliFx.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modulight.Modules.CommandLine;
using Modulight.Modules.Hosting;

namespace Test.CommandLine
{
    class Program
    {
        public static async Task Main(string[] args) => await CreateHostBuilder(args).Build().RunAsyncWithModules();

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
}
