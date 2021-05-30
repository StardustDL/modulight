using Modulight.Modules.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules.CommandLine
{
    /// <summary>
    /// Specifies the interface to build a module manifest.
    /// </summary>
    public interface ICommandLineModuleManifestBuilder : IModuleManifestBuilder<ICommandLineModule, CommandLineModuleManifest>
    {
        /// <summary>
        /// All command types.
        /// </summary>
        IList<Type> Commands { get; }
    }

    class DefaultCommandLineModuleManifestBuilder : ICommandLineModuleManifestBuilder
    {
        public IList<Type> Commands { get; } = new List<Type>();

        public CommandLineModuleManifest Build()
        {
            return new CommandLineModuleManifest
            {
                Commands = Commands.ToArray()
            };
        }
    }
}
