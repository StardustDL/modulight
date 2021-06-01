using Microsoft.JSInterop;
using System.Threading;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    internal class ModuleUILoader
    {
        public const string ResourceTagAttrName = "Modulight_Module_Client_RazorComponents_Resource";

        public ModuleUILoader(IJSModuleProvider<ModuleUILoader> provider)
        {
            Provider = provider;
        }

        public IJSModuleProvider<ModuleUILoader> Provider { get; }

        Task<IJSObjectReference> GetEntryJSModule(CancellationToken cancellationToken = default) => Provider.GetJSModule("module.js", cancellationToken: cancellationToken);

        public async ValueTask CacheDataFromPath(string path, bool forceUpdate = false, CancellationToken cancellationToken = default)
        {
            var js = await GetEntryJSModule(cancellationToken).ConfigureAwait(false);
            await js.InvokeVoidAsync("cacheDataFromPath", cancellationToken, path, forceUpdate);
        }

        public async ValueTask LoadScript(string src, CancellationToken cancellationToken = default)
        {
            var js = await GetEntryJSModule(cancellationToken).ConfigureAwait(false);
            await js.InvokeVoidAsync("loadScript", cancellationToken, src, ResourceTagAttrName).ConfigureAwait(false);
        }

        public async ValueTask LoadStyleSheet(string href, CancellationToken cancellationToken = default)
        {
            var js = await GetEntryJSModule(cancellationToken).ConfigureAwait(false);
            await js.InvokeVoidAsync("loadStyleSheet", cancellationToken, href, ResourceTagAttrName).ConfigureAwait(false);
        }
    }
}
