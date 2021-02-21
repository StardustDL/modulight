using Microsoft.AspNetCore.Components;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// A razor component to set module page title for <see cref="PageParent"/>.
    /// </summary>
    public class PageInfo : ComponentBase
    {
        /// <summary>
        /// Get <see cref="PageParent"/> instance.
        /// </summary>
        [CascadingParameter]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        protected PageParent Parent { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        /// <summary>
        /// Set <seealso cref="PageParent.Title"/>.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        /// <summary>
        /// Set <seealso cref="PageParent.Icon"/>.
        /// </summary>
        [Parameter]
        public string Icon { get; set; } = "";

        /// <summary>
        /// Set <seealso cref="PageParent.IconFragment"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? IconFragment { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.TitleFragment"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? TitleFragment { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.HeaderContent"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? HeaderContent { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.HeaderExtra"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? HeaderExtra { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.SubtitleFragment"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? SubtitleFragment { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.Tags"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? Tags { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.HeaderFooter"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? HeaderFooter { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.Footer"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? Footer { get; set; }

        /// <summary>
        /// Set <seealso cref="PageParent.Breadcrumb"/>.
        /// </summary>
        [Parameter]
        public RenderFragment? Breadcrumb { get; set; }

        /// <inheritdoc/>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Parent.Title = Title;
            Parent.TitleFragment = TitleFragment;
            Parent.Footer = Footer;
            Parent.Breadcrumb = Breadcrumb;
            Parent.HeaderContent = HeaderContent;
            Parent.HeaderExtra = HeaderExtra;
            Parent.Tags = Tags;
            Parent.SubtitleFragment = SubtitleFragment;
            Parent.Icon = Icon;
            Parent.IconFragment = IconFragment;
        }
    }
}
