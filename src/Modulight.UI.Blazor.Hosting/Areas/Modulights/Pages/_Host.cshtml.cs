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
            AppRenderMode = Options.GetRenderMode();
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
