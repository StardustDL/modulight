using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// Provide JS modules.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IJSModuleProvider<T> : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Get a lazy javascript module at /_content/<paramref name="assemblyName"/>/<paramref name="jsPath"/>.
        /// </summary>
        /// <param name="jsPath">Javascript file path.</param>
        /// <param name="assemblyName">Assembly name, null for the assembly which <typeparamref name="T"/> defined.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IJSObjectReference> GetJSModule(string jsPath, string? assemblyName = null, CancellationToken cancellationToken = default);
    }

    internal class JSModuleProvider<T> : IJSModuleProvider<T>
    {
        Dictionary<string, Lazy<Task<IJSObjectReference>>> JSInvokers { get; } = new Dictionary<string, Lazy<Task<IJSObjectReference>>>();

        public JSModuleProvider(IJSRuntime jsRuntime, ILogger<T> logger)
        {
            JSRuntime = jsRuntime;
            Logger = logger;
        }

        /// <summary>
        /// JS runtime.
        /// </summary>
        protected IJSRuntime JSRuntime { get; }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger<T> Logger { get; }

        public Task<IJSObjectReference> GetJSModule(string jsPath, string? assemblyName = null, CancellationToken cancellationToken = default)
        {
            if (assemblyName is null)
                assemblyName = typeof(T).Assembly.GetName().Name ?? "";

            string id = $"{assemblyName}/{jsPath}";

            if (!JSInvokers.ContainsKey(id))
            {
                Logger.LogInformation($"Create JS invoker: {id}.");
                JSInvokers.Add(id, new(() =>
                    JSRuntime.InvokeAsync<IJSObjectReference>("import", cancellationToken, $"./_content/{id}").AsTask()));
            }

            return JSInvokers[id].Value;
        }

        #region Dispose

        /// <inheritdoc/>
        protected async ValueTask DisposeAsyncCore()
        {
            foreach (var invoker in JSInvokers)
            {
                if (invoker.Value.IsValueCreated)
                {
                    Logger.LogInformation($"Dispose JS invoker: {invoker.Key}.");
                    var value = await invoker.Value.Value.ConfigureAwait(false);
                    await value.DisposeAsync().ConfigureAwait(false);
                }
            }
            JSInvokers.Clear();
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        private bool _disposedValue;

        /// <inheritdoc/>
        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var invoker in JSInvokers)
                    {
                        if (invoker.Value.IsValueCreated)
                        {
                            Logger.LogInformation($"Dispose JS invoker: {invoker.Key}.");
                            (invoker.Value.Value as IDisposable)?.Dispose();
                        }
                    }
                    JSInvokers.Clear();
                }
                _disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
