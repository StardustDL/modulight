using System.Reflection;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies page provider.
    /// </summary>
    public interface IPageProvider
    {
        /// <summary>
        /// Get module UI route root path, such as home, search, and so on.
        /// Use <see cref="string.Empty"/> for no page module.
        /// </summary>
        string RootPath { get; }

        /// <summary>
        /// Check if a path is belongs to this module UI.
        /// </summary>
        /// <param name="path">Route path.</param>
        /// <returns></returns>
        bool Contains(string path);
    }

    /// <summary>
    /// Default implement for <see cref="IPageProvider"/>.
    /// </summary>
    public abstract class PageProvider : IPageProvider
    {
        /// <summary>
        /// 
        /// </summary>
        protected PageProvider()
        {
            RootPath = "";
            var type = GetType();
            {
                var attr = type.GetCustomAttribute<ModulePageRootPathAttribute>();
                if (attr is not null)
                    RootPath = attr.RootPath;
            }
        }

        /// <inheritdoc/>
        public string RootPath { get; protected set; }

        /// <inheritdoc/>
        public virtual bool Contains(string path)
        {
            if (RootPath is "")
            {
                return true;
            }
            path = path.Trim('/') + "/";
            return path.StartsWith($"{RootPath}/");
        }
    }
}
