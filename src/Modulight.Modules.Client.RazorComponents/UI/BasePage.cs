using Microsoft.AspNetCore.Components;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// Base class for module UI pages.
    /// </summary>
    public abstract class BasePage : ComponentBase
    {
        /// <summary>
        /// Get <see cref="PageParent"/> instance.
        /// </summary>
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        [CascadingParameter]
        protected PageParent Parent { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        /// <summary>
        /// Get current page's module.
        /// </summary>
        /// <returns></returns>
        public abstract IRazorComponentClientModule GetModule();

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            Parent.Module = GetModule();
            base.OnInitialized();
        }
    }

    /// <summary>
    /// Base class for module UI pages with typed module.
    /// </summary>
    public abstract class BasePage<TModule> : BasePage where TModule : IRazorComponentClientModule
    {
        /// <summary>
        /// Get current page's typed module.
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        [Inject]
        public TModule Module { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        /// <inheritdoc/>
        public override IRazorComponentClientModule GetModule() => Module;
    }
}
