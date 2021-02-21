using Microsoft.AspNetCore.Components;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// Container for module pages.
    /// Provide page services to interop with host UI.
    /// </summary>
    public partial class PageParent : ComponentBase
    {
        private string _title = "", _icon = "";
        private IRazorComponentClientModule? _module;
        private RenderFragment? _titleFragment, _breadcrumb, _subtitleFragment, _tags, _footer, _headerContent, _headerExtra, _iconFragment;
        private RenderFragment? _headerFooter;

        /// <summary>
        /// Get or set page title.
        /// </summary>
        public string Title
        {
            get => _title; set
            {
                if (_title != value)
                {
                    _title = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page icon.
        /// </summary>
        public string Icon
        {
            get => _icon; set
            {
                if (_icon != value)
                {
                    _icon = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page icon render fragment.
        /// </summary>
        public RenderFragment? IconFragment
        {
            get => _iconFragment; set
            {
                if (_iconFragment != value)
                {
                    _iconFragment = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page header title render fragment.
        /// </summary>
        public RenderFragment? TitleFragment
        {
            get => _titleFragment; set
            {
                if (_titleFragment != value)
                {
                    _titleFragment = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page header subtitle render fragment.
        /// </summary>
        public RenderFragment? SubtitleFragment
        {
            get => _subtitleFragment; set
            {
                if (_subtitleFragment != value)
                {
                    _subtitleFragment = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page header breadcrumb render fragment.
        /// </summary>
        public RenderFragment? Breadcrumb
        {
            get => _breadcrumb; set
            {
                if (_breadcrumb != value)
                {
                    _breadcrumb = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page header tags render fragment.
        /// </summary>
        public RenderFragment? Tags
        {
            get => _tags; set
            {
                if (_tags != value)
                {
                    _tags = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page footer render fragment.
        /// </summary>
        public RenderFragment? Footer
        {
            get => _footer; set
            {
                if (_footer != value)
                {
                    _footer = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page header's footer render fragment.
        /// </summary>
        public RenderFragment? HeaderFooter
        {
            get => _headerFooter; set
            {
                if (_headerFooter != value)
                {
                    _headerFooter = value;
                    StateHasChanged();
                }

            }
        }

        /// <summary>
        /// Get or set page header content render fragment.
        /// </summary>
        public RenderFragment? HeaderContent
        {
            get => _headerContent; set
            {
                if (_headerContent != value)
                {
                    _headerContent = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set page header extra render fragment.
        /// </summary>
        public RenderFragment? HeaderExtra
        {
            get => _headerExtra; set
            {
                if (_headerExtra != value)
                {
                    _headerExtra = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set current module.
        /// </summary>
        public IRazorComponentClientModule? Module
        {
            get => _module; set
            {
                if (_module != value)
                {
                    _module = value;
                    StateHasChanged();
                }
            }
        }
    }
}
