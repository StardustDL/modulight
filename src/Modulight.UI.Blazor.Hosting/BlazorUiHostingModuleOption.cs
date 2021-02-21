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
    }
}
