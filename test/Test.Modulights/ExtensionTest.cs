using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System;

namespace Test.Modulights
{
    [TestClass]
    public class ExtensionTest
    {
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }

        class Startup : ModuleStartup
        {

        }

        [TestMethod]
        public void Type()
        {
            typeof(TestModule).EnsureModule();
            Assert.ThrowsException<Exception>(() => typeof(object).EnsureModule());

            typeof(Startup).EnsureModuleStartup();
            Assert.ThrowsException<Exception>(() => typeof(object).EnsureModuleStartup());
        }
    }
}
