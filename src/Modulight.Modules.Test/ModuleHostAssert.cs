using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulight.Modules.Test
{
    public static class ModuleHostAssert
    {
        public static IModuleHost HasModule(this IModuleHost host, Type type)
        {
            Assert.IsTrue(host.DefinedModules.Contains(type), $"No this module {type.FullName}");
            host.HasService(type);
            return host;
        }

        public static IModuleHost HasModule<T>(this IModuleHost host) where T : IModule => host.HasModule(typeof(T));

        public static T EnsureGetLoadedModule<T>(this IModuleHost host) where T : IModule
        {
            return host.HasLoadedModule<T>().GetModule<T>();
        }

        public static IModuleHost HasLoadedModule(this IModuleHost host, Type type)
        {
            Assert.IsTrue(host.LoadedModules.Any(x => x.GetType() == type), $"No this loaded module {type.FullName}");
            return host;
        }

        public static IModuleHost HasLoadedModule<T>(this IModuleHost host) where T : IModule => host.HasLoadedModule(typeof(T));

        public static IModuleHost HasService(this IModuleHost host, Type type)
        {
            Assert.IsNotNull(host.Services.GetService(type), $"No this service {type.FullName}");
            return host;
        }

        public static IModuleHost HasService<T>(this IModuleHost host) where T : notnull => host.HasService(typeof(T));

        public static T EnsureGetService<T>(this IModuleHost host) where T : notnull
        {
            return host.HasService<T>().Services.GetRequiredService<T>();
        }

        public static IModuleHost HasDependencies<T>(this IModuleHost host) where T : IModule
        {
            host.HasModule<T>();
            var manifest = host.GetManifest<T>();
            foreach (var item in manifest.Dependencies)
                host.HasModule(item);
            return host;
        }

        public static IModuleHost HasServices<T>(this IModuleHost host) where T : IModule
        {
            host.HasModule<T>();
            var manifest = host.GetManifest<T>();
            foreach (var item in manifest.Services)
                host.HasService(item.ServiceType);
            return host;
        }
    }
}
