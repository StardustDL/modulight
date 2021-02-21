using Modulight.Modules.Server.AspNet;

namespace Modulight.Modules.Test.Context
{
    public class AspNetServerModuleTestContext : ModuleTestContext
    {
        public AspNetServerModuleTestContext() : base()
        {
        }
    }

    public class AspNetServerModuleTestContext<T> : AspNetServerModuleTestContext where T : IAspNetServerModule
    {
        public AspNetServerModuleTestContext() : base()
        {
            WithModule<T>();
        }
    }
}
