using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules.Hosting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class LifetimeTest
    {
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }

        [TestMethod]
        public async Task Test()
        {
            var context = new ModuleTestContext<TestModule>();
            await context.UseHost(host =>
            {
                var module = host.EnsureGetService<TestModule>();
                Assert.IsFalse(module.HasInitialized);
                Assert.IsFalse(module.HasShutdowned);
            });
            await context.Run(host =>
            {
                var module = host.EnsureGetLoadedModule<TestModule>();
                Assert.IsTrue(module.HasInitialized);
                Assert.IsFalse(module.HasShutdowned);
            }, host =>
            {
                var module = host.EnsureGetLoadedModule<TestModule>();
                Assert.IsTrue(module.HasInitialized);
                Assert.IsTrue(module.HasShutdowned);
            });
        }
    }
}
