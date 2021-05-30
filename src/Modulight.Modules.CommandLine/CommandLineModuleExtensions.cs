using CliFx;
using CliFx.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modulight.Modules.CommandLine;
using System;
using System.Linq;
using System.Reflection;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Extension methods for commandline modules.
    /// </summary>
    public static class CommandLineModuleExtensions
    {
        /// <summary>
        /// Use building plugin for commandline modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseCommandLineModules(this IModuleHostBuilder modules)
        {
            return modules.ConfigureBuilderServices(services =>
            {
                services.TryAddTransient<ICommandLineModuleManifestBuilder, DefaultCommandLineModuleManifestBuilder>();
            }).UsePlugin<CommandLineModulePlugin>().AddModule<CommandLineCoreModule>();
        }

        /// <summary>
        /// Get commandline module host from service provider.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static ICommandLineModuleCollection GetCommandLineModuleCollection(this IModuleHost host) => host.Services.GetRequiredService<ICommandLineModuleCollection>();

        /// <summary>
        /// Test a type is a command type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCommand(this Type type)
        {
            // From CliFx source code AddCommandsFromThisAssembly()

            if (type.GetInterfaces().Contains(typeof(ICommand)) && type.IsDefined(typeof(CommandAttribute)) && !type.IsAbstract)
            {
                return !type.IsInterface;
            }

            return false;
        }

        /// <summary>
        /// Ensure a type is a command type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void EnsureCommand(this Type type)
        {
            if (!type.IsCommand())
                throw new Exception($"{type.FullName} is not a command typed.");
        }

        /// <summary>
        /// Add a command type to manifest.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICommandLineModuleManifestBuilder AddCommand(this ICommandLineModuleManifestBuilder builder, Type type)
        {
            type.EnsureCommand();
            builder.Commands.Add(type);
            return builder;
        }

        /// <summary>
        /// Add a command type to manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ICommandLineModuleManifestBuilder AddCommand<T>(this ICommandLineModuleManifestBuilder builder) where T : ICommand => builder.AddCommand(typeof(T));

        /// <summary>
        /// Configure the builder by default from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICommandLineModuleManifestBuilder WithDefaultsFromModuleType(this ICommandLineModuleManifestBuilder builder, Type type)
        {
            if (type.IsDefined(typeof(CommandsFromAssemblyAttribute)))
            {
                builder.WithCommandsFromModuleAssembly(type);
            }

            var cmdAttrs = type.GetCustomAttributes<CommandFromAttribute>(true);

            foreach(var attr in cmdAttrs)
            {
                builder.AddCommand(attr.Type);
            }

            return builder;
        }

        /// <summary>
        /// Add commands from the assembly of module.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICommandLineModuleManifestBuilder WithCommandsFromModuleAssembly(this ICommandLineModuleManifestBuilder builder, Type type)
        {
            foreach (Type item in type.Assembly.ExportedTypes.Where(x => x.IsCommand()))
            {
                builder.AddCommand(item);
            }
            return builder;
        }
    }
}
