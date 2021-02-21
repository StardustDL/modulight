using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Test.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class PluginTest
    {
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }

        class TestPlugin : ModuleHostBuilderPlugin
        {
        }

        [TestMethod]
        public async Task Test()
        {
            var context = new ModuleTestContext<TestModule>().WithPlugin<TestPlugin>();
            await context.Run();
        }
    }
}
