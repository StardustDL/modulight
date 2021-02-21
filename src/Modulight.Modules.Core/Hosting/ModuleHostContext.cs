using System;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{
    internal class ModuleHostContext : IAsyncDisposable
    {
        public ModuleHostContext(IModuleHost host) => Host = host;

        IModuleHost Host { get; }

        public async ValueTask DisposeAsync() => await Host.Shutdown().ConfigureAwait(false);
    }
}