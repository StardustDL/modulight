using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Server.AspNet
{
    /// <summary>
    /// Specifies the contract for aspnet modules.
    /// </summary>
    public interface IAspNetServerModule : IModule
    {
        /// <summary>
        /// Use all registered module's middlewares.
        /// Used for <see cref="UseMiddlewareExtensions.UseMiddleware(IApplicationBuilder, Type, object[])"/> 
        /// </summary>
        /// <param name="builder"></param>
        void UseMiddleware(IApplicationBuilder builder);

        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        void MapEndpoint(IEndpointRouteBuilder builder);
    }

    /// <summary>
    /// Basic implement for <see cref="IAspNetServerModule"/>
    /// </summary>
    [ModuleDependency(typeof(AspnetServerCoreModule))]
    public abstract class AspNetServerModule : Module, IAspNetServerModule
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected AspNetServerModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public virtual void MapEndpoint(IEndpointRouteBuilder builder) { }

        /// <inheritdoc/>
        public virtual void UseMiddleware(IApplicationBuilder builder) { }
    }

    [Module(Author = "StardustDL", Description = "Provide services for aspnet server modules.", Url = "https://github.com/StardustDL/delights")]
    [ModuleService(typeof(AspNetServerModuleCollection), ServiceType = typeof(IAspNetServerModuleCollection), Lifetime = ServiceLifetime.Singleton)]
    class AspnetServerCoreModule : Module
    {
        public AspnetServerCoreModule(IModuleHost host, IAspNetServerModuleCollection collection) : base(host)
        {
            Collection = collection;
        }

        public IAspNetServerModuleCollection Collection { get; }
    }
}
