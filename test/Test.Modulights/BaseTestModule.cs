﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;

namespace Test.Modulights
{
    abstract class BaseTestModule : Module
    {
        public BaseTestModule(IModuleHost host) : base(host)
        {
        }

        public bool HasInitialized { get; protected set; }

        public bool HasShutdowned { get; protected set; }

        public override Task Initialize(CancellationToken cancellationToken = default)
        {
            Assert.IsFalse(HasInitialized, $"The module {GetType().FullName} has initialized.");
            foreach (var depType in Manifest.Dependencies)
            {
                var dep = (IModule)Host.Services.GetRequiredService(depType);
                if (dep is BaseTestModule bdep)
                {
                    Assert.IsTrue(bdep.HasInitialized, $"The dep {depType.FullName} is not initialized.");
                }
            }
            foreach (var service in Manifest.Services)
            {
                Host.Services.GetRequiredService(service.ServiceType);
            }
            HasInitialized = true;
            return base.Initialize(cancellationToken);
        }

        public override Task Shutdown(CancellationToken cancellationToken = default)
        {
            Assert.IsFalse(HasShutdowned, $"The module {GetType().FullName} has shutdowned.");
            HasShutdowned = true;
            return base.Shutdown(cancellationToken);
        }
    }
}
