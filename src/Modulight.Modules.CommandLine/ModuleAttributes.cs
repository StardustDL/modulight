using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.CommandLine
{
    /// <summary>
    /// Add all commands from module assembly. <seealso cref="CommandLineModuleExtensions.WithCommandsFromModuleAssembly(ICommandLineModuleManifestBuilder, Type)"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CommandsFromAssemblyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public CommandsFromAssemblyAttribute() { }
    }

    /// <summary>
    /// Add command from a command type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class CommandFromAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public CommandFromAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Type of the command.
        /// </summary>
        public Type Type { get; init; }
    }
}
