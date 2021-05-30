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

        /// <summary>
        /// Get the manifest.
        /// </summary>
        GraphQLServerModuleManifest GraphQLServerModuleManifest { get; }
    }

    /// <summary>
    /// Basic implement for <see cref="IGraphQLServerModule"/>
    /// </summary>
    [ModuleDependency(typeof(GraphqlServerCoreModule))]
    public abstract class GraphQLServerModule : Module, IGraphQLServerModule
    {
        /// <summary>
        /// Get the collection.
        /// </summary>
        protected IGraphQLServerModuleCollection Collection { get; }

        readonly Lazy<GraphQLServerModuleManifest> _manifest;

        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected GraphQLServerModule(IModuleHost host) : base(host)
        {
            Collection = host.GetGraphQLServerModuleCollection();
            _manifest = new Lazy<GraphQLServerModuleManifest>(() => Collection.GetManifest(GetType()));
        }

        /// <inheritdoc/>
        public GraphQLServerModuleManifest GraphQLServerModuleManifest => _manifest.Value;

        /// <inheritdoc/>
        public virtual GraphQLEndpointConventionBuilder? MapEndpoint(IEndpointRouteBuilder builder)
        {
            var manifest = Host.GetGraphQLServerModuleCollection().GetManifest(GetType());
            return builder.MapGraphQL(manifest.Endpoint.TrimEnd('/'), manifest.SchemaName);
        }
    }

    [Module(Author = "StardustDL", Description = "Provide services for graphql server modules.", Url = "https://github.com/StardustDL/modulight")]
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
