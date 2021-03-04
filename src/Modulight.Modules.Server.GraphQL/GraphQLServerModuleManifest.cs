using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Manifest for <see cref="GraphQLServerModule"/>.
    /// </summary>
    public record GraphQLServerModuleManifest : ModuleManifest<IGraphQLServerModule>
    {
        /// <summary>
        /// Schema name.
        /// </summary>
        public string SchemaName { get; init; } = "";

        /// <summary>
        /// Endpoint.
        /// </summary>
        public string Endpoint { get; init; } = "";

        /// <summary>
        /// Query type for <see cref="SchemaRequestExecutorBuilderExtensions.AddQueryType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public Type? QueryType { get; init; }

        /// <summary>
        /// Mutation type for <see cref="SchemaRequestExecutorBuilderExtensions.AddMutationType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public Type? MutationType { get; init; }

        /// <summary>
        /// Subscription type for <see cref="SchemaRequestExecutorBuilderExtensions.AddSubscriptionType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public Type? SubscriptionType { get; init; }
    }
}
