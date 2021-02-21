using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Test.Context;
using System;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class StartupTest
    {
        [ModuleStartup(typeof(BasicStartup))]
        class BasicTestModule : BaseTestModule
        {
            public BasicTestModule(IModuleHost host) : base(host)
            {
            }
        }

        [ModuleStartup(typeof(OptionStartup))]
        class OptionTestModule : BaseTestModule
        {
            public OptionTestModule(IModuleHost host) : base(host)
            {
            }
        }

        class StartupOption
        {
            public string Content { get; set; }
        }

        class BasicStartup : ModuleStartup
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddOptions<StartupOption>().Configure(o => o.Content = nameof(BasicStartup));
                base.ConfigureServices(services);
            }
        }

        class OptionStartup : ModuleStartup
        {
            public OptionStartup(IOptions<StartupOption> option) => Option = option.Value;

            public StartupOption Option { get; }

            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddOptions<StartupOption>().Configure(o => o.Content = Option.Content);
                base.ConfigureServices(services);
            }
        }

        [TestMethod]
        public async Task Basic()
        {
            var context = new ModuleTestContext<BasicTestModule>()
                .CheckStartup<BasicTestModule, BasicStartup>();
            await context.Run(host =>
            {
                var option = host.Services.GetService<IOptions<StartupOption>>().Value;
                Assert.AreEqual(nameof(BasicStartup), option.Content);
            });
        }

        [TestMethod]
        public async Task Option()
        {
            const string OptionContent = nameof(Option);
            var context = new ModuleTestContext<OptionTestModule>()
                .CheckStartup<OptionTestModule, OptionStartup>();
            context.ConfigureBuilder(builder =>
                builder.ConfigureBuilderOptions<StartupOption>((o, _) => o.Content = OptionContent));
            await context.Run(host =>
            {
                var option = host.Services.GetService<IOptions<StartupOption>>().Value;
                Assert.AreEqual(OptionContent, option.Content);
            });
        }
    }
}
