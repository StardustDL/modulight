using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Specifies the contract for graphql modules.
    /// </summary>
    public interface IGraphQLServerModule : IModule
    {
        /// <summary>
        /// Map graphql endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        GraphQLEndpointConventionBuilder? MapEndpoint(IEndpointRouteBuilder builder);
    }

    /// <summary>
    /// Basic implement for <see cref="IGraphQLServerModule"/>
    /// </summary>
    [ModuleDependency(typeof(GraphqlServerCoreModule))]
    public abstract class GraphQLServerModule : Module, IGraphQLServerModule
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected GraphQLServerModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public virtual GraphQLEndpointConventionBuilder? MapEndpoint(IEndpointRouteBuilder builder)
        {
            GraphQLModuleTypeAttribute? attribute = GetType().GetCustomAttribute<GraphQLModuleTypeAttribute>();
            if (attribute is not null)
            {
                return builder.MapGraphQL(attribute.Endpoint.TrimEnd('/'), attribute.SchemaName);
            }
            return null;
        }
    }

    [Module(Author = "StardustDL", Description = "Provide services for graphql server modules.", Url = "https://github.com/StardustDL/delights")]
    [ModuleService(typeof(GraphQLServerModuleCollection), ServiceType = typeof(IGraphQLServerModuleCollection), Lifetime = ServiceLifetime.Singleton)]
    class GraphqlServerCoreModule : Module
    {
        public GraphqlServerCoreModule(IModuleHost host, IGraphQLServerModuleCollection collection) : base(host)
        {
            Collection = collection;
        }

        public IGraphQLServerModuleCollection Collection { get; }
    }
}
