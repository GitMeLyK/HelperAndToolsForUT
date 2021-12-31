using System;
using Microsoft.Extensions.DependencyInjection;

using HelperAndToolsForUT.Helper.Abstraction.IOC;

namespace HelperAndToolsForUT.Helper.Extensions.IocExtensions
{

    /// <summary>
    ///     We all love to have modules to simplify registrations on our DI framework of choice.
    ///     To use in combination with Class implmenttive of ModuleBase
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        ///     We all love to have modules to simplify registrations on our DI framework of choice.
        ///     To use in combination with Class implmenttive of ModuleBase
        ///     See tests for examples
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection RegisterModule<T>(this IServiceCollection services, T module)
            where T : Module
        {
            return module.Loader<T>(services);
        }

        /// <summary>
        ///     We all love to have modules to simplify registrations on our DI framework of choice.
        ///     To use in combination with Class implmenttive of ModuleBase
        ///     See tests for examples
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection RegisterModule<T>(this IServiceCollection services)
            where T : Module, new()
        {
            var module = new T();
            return module.Loader<T>(services);
        }

        /// <summary>
        ///     We all love to have modules to simplify registrations on our DI framework of choice.
        ///     To use in combination with Class implmenttive of ModuleBase
        ///     See tests for examples
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection RegisterModule<T>(this IServiceCollection services, Func<IServiceCollection, T> factory)
            where T : Module
        {
            var module = factory(services);
            return module.Loader<T>(services);
        }
    }
}
