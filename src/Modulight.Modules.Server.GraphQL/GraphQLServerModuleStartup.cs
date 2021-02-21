using HotChocolate.Execution.Configuration;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Startup for graphql server module.
    /// </summary>
    public interface IGraphQLServerModuleStartup : IModuleStartup
    {
        /// <summary>
        /// Configure the graphql schema. The module needs <see cref="GraphQLModuleTypeAttribute"/>.
        /// </summary>
        /// <param name="builder"></param>
        void ConfigureGraphQLSchema(IRequestExecutorBuilder builder);
    }

    /// <summary>
    /// Empty implementation for <see cref="IGraphQLServerModuleStartup"/>.
    /// </summary>
    public abstract class GraphQLServerModuleStartup : ModuleStartup, IGraphQLServerModuleStartup
    {
        /// <inheritdoc/>
        public virtual void ConfigureGraphQLSchema(IRequestExecutorBuilder builder) { }
    }
}
