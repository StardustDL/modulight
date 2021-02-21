using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;

namespace Modulight.Modules.Server.GraphQL
{
    internal sealed class GraphQLServerModulePlugin : ModuleHostBuilderPlugin
    {
        public override void AfterModule(ModuleDefinition module, IServiceCollection services)
        {
            if (module.Type.IsModule<IGraphQLServerModule>())
            {
                GraphQLModuleTypeAttribute? attribute = module.Type.GetCustomAttribute<GraphQLModuleTypeAttribute>();
                if (attribute is not null)
                {
                    var builder = services.AddGraphQLServer(attribute.SchemaName);
                    if (attribute.QueryType is not null)
                    {
                        builder.AddQueryType(attribute.QueryType);
                    }
                    if (attribute.MutationType is not null)
                    {
                        builder.AddMutationType(attribute.MutationType);
                    }
                    if (attribute.SubscriptionType is not null)
                    {
                        builder.AddSubscriptionType(attribute.SubscriptionType);
                    }

                    if (module.Startup is IGraphQLServerModuleStartup startup)
                    {
                        startup.ConfigureGraphQLSchema(builder);
                    }

                    builder.AddFiltering().AddSorting().AddProjections();
                }
            }
            base.AfterModule(module, services);
        }
    }
}
