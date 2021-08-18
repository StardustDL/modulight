using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.AspNet;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for aspnet modules.
    /// </summary>
    public static class AspNetServerModuleExtensions
    {
        /// <summary>
        /// Get aspnet module host from service provider.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IAspNetServerModuleCollection GetAspNetServerModuleCollection(this IModuleHost host) => host.Services.GetRequiredService<IAspNetServerModuleCollection>();

        /// <summary>
        /// Use aspnet modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseAspNetServerModules(this IModuleHostBuilder modules) => modules.AddModule<AspnetServerCoreModule>();
    }
}

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for aspnet modules.
    /// </summary>
    public static class AspNetServerModuleExtensions
    {
        /// <summary>
        /// Use all registered aspnet server module's middlewares.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAspNetServerModuleMiddlewares(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetModuleHost().GetAspNetServerModuleCollection().UseMiddlewares(builder);
            return builder;
        }

        /// <summary>
        /// Map all registered aspnet server module's endpoints.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapAspNetServerModuleEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.ServiceProvider.GetModuleHost().GetAspNetServerModuleCollection().MapEndpoints(builder);
            return builder;
        }
    }
}
