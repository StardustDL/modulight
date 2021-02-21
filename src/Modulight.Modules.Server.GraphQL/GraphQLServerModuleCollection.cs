using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Specifies the contract for graphql module hosts.
    /// </summary>
    public interface IGraphQLServerModuleCollection : IModuleCollection<IGraphQLServerModule>
    {
        /// <summary>
        /// Map all registered module's endpoints.
        /// Used in <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="postMapEndpoint">Action to configure mapped GraphQL endpoints.</param>
        void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null);
    }

    internal class GraphQLServerModuleCollection : ModuleHostFilter<IGraphQLServerModule>, IGraphQLServerModuleCollection
    {
        public GraphQLServerModuleCollection(IModuleHost host) : base(host)
        {
        }

        public void MapEndpoints(IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            foreach (var module in LoadedModules)
            {
                var gbuilder = module.MapEndpoint(builder);
                if (gbuilder is not null && postMapEndpoint is not null)
                    postMapEndpoint(module, gbuilder);
            }
        }
    }
}