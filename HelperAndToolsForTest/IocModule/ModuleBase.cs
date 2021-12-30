using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperAndToolsForUT.Helper.IocModule
{
    public abstract class Module
    {
        protected internal Module()
        {
        }

        protected virtual void Load(IServiceCollection services)
        {
        }

        internal IServiceCollection Loader<T>(IServiceCollection services)
            where T : Module
        {
            Load(services);
            return services;
        }
    }
}
