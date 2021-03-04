using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Specifies the object types for GraphQL server.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GraphQLModuleTypeAttribute : Attribute
    {
        /// <summary>
        /// Specifies the object types for GraphQL server.
        /// </summary>
        /// <param name="schemaName">Schema name.</param>
        public GraphQLModuleTypeAttribute(string schemaName)
        {
            SchemaName = schemaName;
            Endpoint = $"/graphql/{SchemaName}";
        }

        /// <summary>
        /// Specifies the object types for GraphQL server.
        /// </summary>
        /// <param name="schemaName">Schema name.</param>
        /// <param name="queryType">Query type.</param>
        public GraphQLModuleTypeAttribute(string schemaName, Type queryType)
        {
            SchemaName = schemaName;
            Endpoint = $"/graphql/{SchemaName}";
            QueryType = queryType;
        }

        /// <summary>
        /// Schema name.
        /// </summary>
        public string SchemaName { get; init; }

        /// <summary>
        /// Endpoint (default as /graphql/<see cref="SchemaName"/>).
        /// </summary>
        public string Endpoint { get; init; }

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
