using CliFx;
using CliFx.Infrastructure;

namespace Modulight.Modules.CommandLine
{
    /// <summary>
    /// Command of module.
    /// </summary>
    public abstract class ModuleCommand : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        protected ModuleCommand()
        {
        }

        /// <inheritdoc/>
        public ValueTask ExecuteAsync(IConsole console) => ExecuteAsync(console, console.RegisterCancellationHandler());

        /// <summary>
        /// Executes the command using the specified implementation of <see cref="IConsole"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract ValueTask ExecuteAsync(IConsole console, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Command of module.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public abstract class ModuleCommand<TModule> : ModuleCommand where TModule : ICommandLineModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        protected ModuleCommand(TModule module) : base()
        {
            Module = module;
        }

        /// <summary>
        /// Module
        /// </summary>
        protected TModule Module { get; }
    }
}
