using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting
{
    /// <summary>
    /// Extension methods for default Blazor UI on WebAssembly host builder.
    /// </summary>
    public static class WebAssemblyHostBuilderExtensions
    {
        /// <summary>
        /// Configure builder for Blazor UI.
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureBlazorUI(this WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<HeadOutlet>("head::after");
        }
    }
}
