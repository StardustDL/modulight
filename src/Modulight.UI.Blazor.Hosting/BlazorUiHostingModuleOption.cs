using Microsoft.AspNetCore.Mvc.Rendering;

namespace Modulight.UI.Blazor.Hosting
{
    /// <summary>
    /// Options for Blazor UI Hosting module.
    /// </summary>
    public class BlazorUiHostingModuleOption
    {
        /// <summary>
        /// Hosting model
        /// </summary>
        public HostingModel HostingModel { get; set; }

        /// <summary>
        /// Enable prerendering.
        /// </summary>
        public bool EnablePrerendering { get; set; } = true;

        /// <summary>
        /// Enable service worker.
        /// </summary>
        public bool EnableServiceWorker { get; set; } = true;

        /// <summary>
        /// Use default blazor hub.
        /// </summary>
        public bool DefaultBlazorHub { get; set; } = true;

        /// <summary>
        /// Use default blazor framework files.
        /// </summary>
        public bool DefaultBlazorFrameworkFiles { get; set; } = true;

        /// <summary>
        /// Get render mode.
        /// </summary>
        /// <returns></returns>
        public RenderMode GetRenderMode()
        {
            return this switch
            {
                { HostingModel: HostingModel.Server, EnablePrerendering: true } => RenderMode.ServerPrerendered,
                { HostingModel: HostingModel.Server, EnablePrerendering: false } => RenderMode.Server,
                { HostingModel: HostingModel.WebView, EnablePrerendering: true } => RenderMode.ServerPrerendered,
                { HostingModel: HostingModel.WebView, EnablePrerendering: false } => RenderMode.Server,
                { HostingModel: HostingModel.Client, EnablePrerendering: true } => RenderMode.WebAssemblyPrerendered,
                { HostingModel: HostingModel.Client, EnablePrerendering: false } => RenderMode.WebAssembly,
                _ => RenderMode.Static,
            };
        }
    }

    /// <summary>
    /// Hosting model
    /// </summary>
    public enum HostingModel
    {
        /// <summary>
        /// Server
        /// </summary>
        Server,
        /// <summary>
        /// Client
        /// </summary>
        Client,
        /// <summary>
        /// WebView
        /// </summary>
        WebView,
    }
}
