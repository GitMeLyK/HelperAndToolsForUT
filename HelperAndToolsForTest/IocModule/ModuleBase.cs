using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperAndToolsForUT.Helper.Abstraction.IOC
{

    /// <summary>
    ///     Abstraction to use for implementation of ModuleBase
    ///     in context with use on Extensions.IocExtensions.ModuleExtensions
    /// </summary>
    public abstract class Module
    {

        /// <summary>
        ///     Abstraction to use for implementation of ModuleBase
        ///     in context with use on Extensions.IocExtensions.ModuleExtensions
        /// </summary>
        protected internal Module()
        {
        }

        /// <summary>
        ///     Method to implment a load of services in module
        /// </summary>
        /// <param name="services"></param>
        protected virtual void Load(IServiceCollection services)
        {
        }

        /// <summary>
        ///     Loader default implementative of Module
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        internal IServiceCollection Loader<T>(IServiceCollection services)
            where T : Module
        {
            Load(services);
            return services;
        }
    }
}
