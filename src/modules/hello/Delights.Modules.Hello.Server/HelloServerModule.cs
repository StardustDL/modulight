using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.GraphQL;
using System.Collections.Generic;
using System.Linq;

namespace Delights.Modules.Hello.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddHelloServerModule(this IModuleHostBuilder builder)
        {
            builder.AddModule<HelloServerModule>();
            return builder;
        }
    }

    [Module(Url = SharedManifest.Url, Author = SharedManifest.Author, Description = SharedManifest.Description)]
    [GraphQLModuleType("Hello", typeof(ModuleQuery))]
    [ModuleService(typeof(ModuleService))]
    public class HelloServerModule : GraphQLServerModule
    {
        public HelloServerModule(IModuleHost host) : base(host)
        {
        }
    }

    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Message> GetMessages([Service] ModuleService service)
        {
            service.Logger.LogInformation(nameof(GetMessages));
            return service.Messages.AsQueryable();
        }
    }

    public record Message
    {
        public string Content { get; init; } = "";
    }

    public class ModuleService
    {
        public ModuleService(ILogger<HelloServerModule> logger) => Logger = logger;

        public ILogger<HelloServerModule> Logger { get; private set; }

        public List<Message> Messages { get; } = new List<Message>() {
            new Message { Content = "Message 1" },
            new Message { Content = "Message 2" },
        };
    }
}
