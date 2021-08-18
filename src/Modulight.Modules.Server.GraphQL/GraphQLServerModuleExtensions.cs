using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.GraphQL;
using System.Reflection;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for graphql modules.
    /// </summary>
    public static class GraphQLServerModuleExtensions
    {
        /// <summary>
        /// Use building plugin for graphql modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseGraphQLServerModules(this IModuleHostBuilder modules)
        {
            return modules.ConfigureBuilderServices(services =>
            {
                services.TryAddTransient<IGraphQLServerModuleManifestBuilder, DefaultGraphQLServerModuleManifestBuilder>();
            }).UsePlugin<GraphQLServerModulePlugin>().AddModule<GraphqlServerCoreModule>();
        }

        /// <summary>
        /// Get graphql module host from service provider.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IGraphQLServerModuleCollection GetGraphQLServerModuleCollection(this IModuleHost host) => host.Services.GetRequiredService<IGraphQLServerModuleCollection>();

        /// <summary>
        /// Configure the builder by default from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IGraphQLServerModuleManifestBuilder WithDefaultsFromModuleType(this IGraphQLServerModuleManifestBuilder builder, Type type)
        {
            {
                GraphQLModuleTypeAttribute? attribute = type.GetCustomAttribute<GraphQLModuleTypeAttribute>();
                if (attribute is not null)
                {
                    builder.SchemaName = attribute.SchemaName;
                    builder.Endpoint = attribute.Endpoint;
                    builder.QueryType = attribute.QueryType;
                    builder.MutationType = attribute.MutationType;
                    builder.SubscriptionType = attribute.SubscriptionType;
                }
            }
            return builder;
        }
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
