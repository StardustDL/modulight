using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;

namespace Modulight.Modules.Test.Context
{
    public static class ContextExtensions
    {
        public static ModuleHostBuilderLog GetBuilderLog(this IModuleHost host) => host.Services.GetRequiredService<ModuleHostBuilderLog>();
    }
}
