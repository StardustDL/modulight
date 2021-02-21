using Modulight.Modules.Client.RazorComponents;

namespace Modulight.Modules.Test.Context
{
    public class RazorComponentClientModuleTestContext : ModuleTestContext
    {
        public RazorComponentClientModuleTestContext() : base()
        {
        }
    }

    public class RazorComponentClientModuleTestContext<T> : RazorComponentClientModuleTestContext where T : IRazorComponentClientModule
    {
        public RazorComponentClientModuleTestContext() : base()
        {
            WithModule<T>();
        }
    }
}
