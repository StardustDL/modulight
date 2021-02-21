using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Server.AspNet
{
    /// <summary>
    /// Specifies the contract for aspnet module hosts.
    /// </summary>
    public interface IAspNetServerModuleCollection : IModuleCollection<IAspNetServerModule>
    {
        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="postMapEndpoint">Action to configure mapped GraphQL endpoints.</param>
        void MapEndpoints(IEndpointRouteBuilder builder, Action<IAspNetServerModule, IEndpointRouteBuilder>? postMapEndpoint = null);

        /// <summary>
        /// Use all registered module's middlewares.
        /// Used for <see cref="UseMiddlewareExtensions.UseMiddleware(IApplicationBuilder, Type, object[])"/> 
        /// </summary>
        /// <param name="builder"></param>
        void UseMiddlewares(IApplicationBuilder builder);
    }

    internal class AspNetServerModuleCollection : ModuleHostFilter<IAspNetServerModule>, IAspNetServerModuleCollection
    {
        public AspNetServerModuleCollection(IModuleHost host) : base(host)
        {
        }

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IAspNetServerModule, IEndpointRouteBuilder>? postMapEndpoint = null)
        {
            foreach (var module in LoadedModules)
            {
                module.MapEndpoint(builder);
                if (postMapEndpoint is not null)
                    postMapEndpoint(module, builder);
            }
        }

        public void UseMiddlewares(IApplicationBuilder builder)
        {
            foreach (var module in LoadedModules)
            {
                module.UseMiddleware(builder);
            }
        }
    }
}