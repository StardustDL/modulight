using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Specifies the interface to build a module manifest.
    /// </summary>
    public interface IGraphQLServerModuleManifestBuilder : IModuleManifestBuilder<IGraphQLServerModule, GraphQLServerModuleManifest>
    {
        /// <summary>
        /// Schema name.
        /// </summary>
        string SchemaName { get; set; }

        /// <summary>
        /// Endpoint.
        /// </summary>
        string Endpoint { get; set; }

        /// <summary>
        /// Query type for <see cref="SchemaRequestExecutorBuilderExtensions.AddQueryType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        Type? QueryType { get; set; }

        /// <summary>
        /// Mutation type for <see cref="SchemaRequestExecutorBuilderExtensions.AddMutationType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        Type? MutationType { get; set; }

        /// <summary>
        /// Subscription type for <see cref="SchemaRequestExecutorBuilderExtensions.AddSubscriptionType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        Type? SubscriptionType { get; set; }
    }

    class DefaultGraphQLServerModuleManifestBuilder : IGraphQLServerModuleManifestBuilder
    {
        /// <summary>
        /// Schema name.
        /// </summary>
        public string SchemaName { get; set; } = "";

        /// <summary>
        /// Endpoint.
        /// </summary>
        public string Endpoint { get; set; } = "";

        /// <summary>
        /// Query type for <see cref="SchemaRequestExecutorBuilderExtensions.AddQueryType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public Type? QueryType { get; set; }

        /// <summary>
        /// Mutation type for <see cref="SchemaRequestExecutorBuilderExtensions.AddMutationType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public Type? MutationType { get; set; }

        /// <summary>
        /// Subscription type for <see cref="SchemaRequestExecutorBuilderExtensions.AddSubscriptionType(IRequestExecutorBuilder, Type)"/>.
        /// </summary>
        public Type? SubscriptionType { get; set; }

        public GraphQLServerModuleManifest Build()
        {
            return new GraphQLServerModuleManifest
            {
                SchemaName = SchemaName,
                Endpoint = Endpoint,
                QueryType = QueryType,
                MutationType = MutationType,
                SubscriptionType = SubscriptionType
            };
        }
    }
}
