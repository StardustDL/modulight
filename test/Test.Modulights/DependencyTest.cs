using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class DependencyTest
    {
        [ModuleDependency(typeof(TestDepModule))]
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }


        [ModuleDependency(typeof(TestDepDepModule))]
        class TestDepModule : BaseTestModule
        {
            public TestDepModule(IModuleHost host) : base(host)
            {
            }
        }

        class TestDepDepModule : BaseTestModule
        {
            public TestDepDepModule(IModuleHost host) : base(host)
            {
            }
        }

        [TestMethod]
        public async Task Test()
        {
            var context = new ModuleTestContext<TestModule>();
            await context.UseHost(host =>
               {
                   host.HasModule<TestDepDepModule>();
                   host.HasModule<TestDepModule>();
               });
            await context.Run();
        }
    }
}
