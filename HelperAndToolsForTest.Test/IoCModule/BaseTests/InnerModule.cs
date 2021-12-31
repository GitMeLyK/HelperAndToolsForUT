using System;
using Microsoft.Extensions.DependencyInjection;

using HelperAndToolsForUT.Helper.Abstraction.IOC;

namespace HelperAndToolsForUT.Helper.Test.IocModule.BaseTests
{

    public class InnerModule : Module
    {
        protected override void Load(IServiceCollection services)
        {
            base.Load(services);
            services.AddSingleton<IService, ServiceImpl2>();
        }
    }
}
