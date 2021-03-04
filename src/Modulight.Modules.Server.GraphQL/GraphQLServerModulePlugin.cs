using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;

namespace Modulight.Modules.Server.GraphQL
{
    internal sealed class GraphQLServerModulePlugin : ModuleHostBuilderPlugin
    {
        public GraphQLServerModulePlugin(IServiceProvider builderServices) => BuilderServices = builderServices;

        IServiceProvider BuilderServices { get; }

        public override void AfterModule(ModuleDefinition module, IServiceCollection services)
        {
            if (module.Type.IsModule<IGraphQLServerModule>())
            {
                var manifestBuilder = BuilderServices.GetRequiredService<IGraphQLServerModuleManifestBuilder>();
                manifestBuilder.WithDefaultsFromModuleType(module.Type);

                {
                    if (module.Startup is IGraphQLServerModuleStartup startup)
                    {
                        startup.ConfigureGraphQLServerModuleManifest(manifestBuilder);
                    }
                }

                var manifest = manifestBuilder.Build();

                services.RegisterModuleManifest(new ModuleManifestServiceEntry<IGraphQLServerModule, GraphQLServerModuleManifest>(module.Type, manifest));

                var builder = services.AddGraphQLServer(manifest.SchemaName);
                if (manifest.QueryType is not null)
                {
                    builder.AddQueryType(manifest.QueryType);
                }
                if (manifest.MutationType is not null)
                {
                    builder.AddMutationType(manifest.MutationType);
                }
                if (manifest.SubscriptionType is not null)
                {
                    builder.AddSubscriptionType(manifest.SubscriptionType);
                }

                {
                    if (module.Startup is IGraphQLServerModuleStartup startup)
                    {
                        startup.ConfigureGraphQLSchema(builder);
                    }
                }

                builder.AddFiltering().AddSorting().AddProjections();
            }
            base.AfterModule(module, services);
        }
    }
}
