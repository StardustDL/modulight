using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Modulight.UI.Blazor.Hosting.Areas.Modulights.Pages
{
    public class HostModel : PageModel
    {
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

        public BlazorUiHostingModuleOption Options { get; }

        public RenderMode AppRenderMode { get; }
    }
}
