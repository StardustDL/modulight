using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using System;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class ServiceOptionTest
    {
        [ModuleService(typeof(TestDirectModuleService))]
        [ModuleService(typeof(TestInnerModuleService), ServiceType = typeof(ITestInnerModuleService))]
        [ModuleService(typeof(TestSingletonModuleService), Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
        [ModuleOption(typeof(TestModuleOption))]
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }

        class TestDirectModuleService
        {

        }

        class TestSingletonModuleService
        {

        }

        class TestInnerModuleService : ITestInnerModuleService
        {

        }

        interface ITestInnerModuleService
        {
        }

        class TestModuleOption
        {

        }

        class TestUndefModuleOption
        {

        }

        [TestMethod]
        public async Task Test()
        {
            var context = new ModuleTestContext<TestModule>();
            await context.Run(host =>
            {
                var module = host.EnsureGetLoadedModule<TestModule>();

                module.GetService<TestSingletonModuleService>(host.Services);
                using var scope = host.Services.CreateScope();
                module.GetService<TestDirectModuleService>(scope.ServiceProvider);
                module.GetService<ITestInnerModuleService>(scope.ServiceProvider);
                Assert.ThrowsException<Exception>(() =>
                {
                    module.GetService<TestInnerModuleService>(scope.ServiceProvider);
                });
                module.GetOption<TestModuleOption>(scope.ServiceProvider);
                Assert.ThrowsException<Exception>(() =>
                {
                    module.GetOption<TestUndefModuleOption>(scope.ServiceProvider);
                });
            });
        }
    }
}
