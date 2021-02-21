using Modulight.Modules.Server.AspNet;
using Modulight.Modules.Server.GraphQL;

namespace Modulight.Modules.Test.Context
{
    public class GraphQLServerModuleTestContext : ModuleTestContext
    {
        public GraphQLServerModuleTestContext() : base()
        {
            ConfigureBuilder(builder => builder.UseGraphQLServerModules());
        }
    }

    public class GraphQLServerModuleTestContext<T> : GraphQLServerModuleTestContext where T : IGraphQLServerModule
    {
        public GraphQLServerModuleTestContext() : base()
        {
            WithModule<T>();
        }
    }
}
