using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class ManifestTest
    {
        [Module(Author = ManifestString,
            Description = ManifestString,
            DisplayName = ManifestString,
            Name = ManifestString,
            Url = ManifestString,
            Version = ManifestString)]
        class TestModule : BaseTestModule
        {
            public const string ManifestString = nameof(TestModule);

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
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Author);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Description);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.DisplayName);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Url);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Version);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Name);
            });
        }
    }
}
