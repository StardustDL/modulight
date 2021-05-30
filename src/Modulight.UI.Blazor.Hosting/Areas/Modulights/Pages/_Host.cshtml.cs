using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Modulight.UI.Blazor.Hosting.Areas.Modulights.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public class HostModel : PageModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public HostModel(IOptions<BlazorUiHostingModuleOption> options)
        {
            Options = options.Value;
            AppRenderMode = Options switch
            {
                { HostingModel: HostingModel.Server, EnablePrerendering: true } => RenderMode.ServerPrerendered,
                { HostingModel: HostingModel.Server, EnablePrerendering: false } => RenderMode.Server,
                { HostingModel: HostingModel.Client, EnablePrerendering: true } => RenderMode.WebAssemblyPrerendered,
                { HostingModel: HostingModel.Client, EnablePrerendering: false } => RenderMode.WebAssembly,
                _ => RenderMode.Static,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public BlazorUiHostingModuleOption Options { get; }

        /// <summary>
        /// 
        /// </summary>
        public RenderMode AppRenderMode { get; }
    }
}
