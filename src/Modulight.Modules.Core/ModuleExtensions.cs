using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modulight.Modules
{
    /// <summary>
    /// Extension methods for modules.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Get all modules in given type.
        /// </summary>
        /// <typeparam name="T">Target module type.</typeparam>
        /// <param name="this">Module list.</param>
        /// <returns></returns>
        public static IEnumerable<T> AllSpecifyModules<T>(this IEnumerable<IModule> @this) => @this.Where(m => m is T).Select(m => (T)m);

        /// <summary>
        /// Get assembly name from type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAssemblyName(this Type type) => type.Assembly.GetName().Name!;

        /// <summary>
        /// Test a type is a module type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static bool IsModule(this Type type, Type? moduleType = null) => type.IsAssignableTo(moduleType ?? typeof(IModule));

        /// <summary>
        /// Test a type is a specified module type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsModule<T>(this Type type) where T : IModule => type.IsModule(typeof(T));

        /// <summary>
        /// Ensure a type is a module type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void EnsureModule(this Type type) => type.EnsureModule<IModule>();

        /// <summary>
        /// Ensure a type is a specified module type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public static void EnsureModule<T>(this Type type) where T : IModule
        {
            if (!type.IsModule<T>())
                throw new Exception($"{type.FullName} is not a module typed {typeof(T).FullName}.");
        }

        /// <summary>
        /// Test a type is a module startup type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsModuleStartup(this Type type) => type.IsModuleStartup<IModuleStartup>();

        /// <summary>
        /// Test a type is a specified module startup type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsModuleStartup<T>(this Type type) where T : IModuleStartup => type.IsAssignableTo(typeof(T));

        /// <summary>
        /// Ensure a type is a module startup type.
        /// </summary>
        /// <param name="type"></param>
        public static void EnsureModuleStartup(this Type type) => type.EnsureModuleStartup<IModuleStartup>();

        /// <summary>
        /// Ensure a type is a specified module startup type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public static void EnsureModuleStartup<T>(this Type type) where T : IModuleStartup
        {
            if (!type.IsModuleStartup<T>())
                throw new Exception($"{type.FullName} is not a module startup typed {typeof(T).FullName}.");
        }

        /// <summary>
        /// Add service to manifest.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static IModuleManifestBuilder WithService(this IModuleManifestBuilder builder, ModuleServiceDescriptor descriptor)
        {
            builder.Services.Add(descriptor);
            return builder;
        }

        /// <summary>
        /// Add option to manifest.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IModuleManifestBuilder WithOption(this IModuleManifestBuilder builder, Type type)
        {
            builder.Options.Add(type);
            return builder;
        }

        /// <summary>
        /// Add option to manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IModuleManifestBuilder WithOption<T>(this IModuleManifestBuilder builder) => builder.WithOption(typeof(T));

        /// <summary>
        /// Add dependency to manifest.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IModuleManifestBuilder WithDependency(this IModuleManifestBuilder builder, Type type)
        {
            type.EnsureModule();
            builder.Dependencies.Add(type);
            return builder;
        }

        /// <summary>
        /// Add dependency to manifest.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IModuleManifestBuilder WithDependency<T>(this IModuleManifestBuilder builder) where T : IModule => builder.WithDependency(typeof(T));

        /// <summary>
        /// Configure the builder by default from attributes.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IModuleManifestBuilder WithDefaultsFromModuleType(this IModuleManifestBuilder builder, Type type)
        {
            // Split by upper chars
            static string[] GenerateName(string typeName)
            {
                if (typeName.EndsWith("Module"))
                    typeName = typeName[0..^6];
                List<int> splitIndexs;
                {
                    SortedSet<int> indexSet = new SortedSet<int>(new[] { 0, typeName.Length });
                    foreach (int i in Enumerable.Range(0, typeName.Length))
                    {
                        if (char.IsUpper(typeName[i]))
                            indexSet.Add(i);
                    }
                    splitIndexs = indexSet.ToList();
                }
                List<string> names = new List<string>();
                foreach (int i in Enumerable.Range(0, splitIndexs.Count - 1))
                {
                    names.Add(typeName[splitIndexs[i]..splitIndexs[i + 1]]);
                }
                return names.ToArray();
            }
            var moduleAttr = type.GetCustomAttribute<ModuleAttribute>(true);
            var serviceAttr = type.GetCustomAttributes<ModuleServiceAttribute>(true);
            var optionAttr = type.GetCustomAttributes<ModuleOptionAttribute>(true);
            var depAttr = type.GetCustomAttributes<ModuleDependencyAttribute>(true);

            builder.Name = moduleAttr?.Name ?? string.Concat(GenerateName(type.Name));
            builder.DisplayName = moduleAttr?.DisplayName ?? string.Join(' ', GenerateName(type.Name));
            builder.Version = moduleAttr?.Version ?? type.Assembly.GetName().Version?.ToString() ?? "0.0.0.0";
            builder.Author = moduleAttr?.Author ?? "Anonymous";
            builder.Description = moduleAttr?.Description ?? "";
            builder.Url = moduleAttr?.Url ?? "";
            foreach (var item in serviceAttr)
            {
                var d = new ModuleServiceDescriptor(
                    item.ImplementationType,
                    item.ServiceType ?? item.ImplementationType,
                    item.Lifetime,
                    item.RegisterBehavior);
                builder.WithService(d);
            }
            foreach (var item in optionAttr)
            {
                builder.WithOption(item.OptionType);
            }
            foreach (var item in depAttr)
            {
                builder.WithDependency(item.ModuleType);
            }
            return builder;
        }
    }
}
