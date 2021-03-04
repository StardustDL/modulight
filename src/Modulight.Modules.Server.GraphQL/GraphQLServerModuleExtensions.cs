using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.GraphQL;
using System;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for aspnet modules.
    /// </summary>
    public static class GraphQLServerModuleExtensions
    {
        /// <summary>
        /// Use building plugin for graphql modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseGraphQLServerModules(this IModuleHostBuilder modules) => modules.UsePlugin<GraphQLServerModulePlugin>();


        /// <summary>
        /// Get graphql module host from service provider.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IGraphQLServerModuleCollection GetGraphQLServerModuleCollection(this IModuleHost host) => host.Services.GetRequiredService<IGraphQLServerModuleCollection>();
    }
}

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for graphql modules.
    /// </summary>
    public static class GraphQLServerModuleExtensions
    {
        /// <summary>
        /// Map all registered graphql server module's endpoints.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="postMapEndpoint">Action to configure mapped GraphQL endpoints.</param>
        /// <returns></returns>
        public static IEndpointRouteBuilder MapGraphQLServerModuleEndpoints(this IEndpointRouteBuilder builder, Action<IGraphQLServerModule, GraphQLEndpointConventionBuilder>? postMapEndpoint = null)
        {
            builder.ServiceProvider.GetModuleHost().GetGraphQLServerModuleCollection().MapEndpoints(builder, postMapEndpoint);
            return builder;
        }
    }
}
