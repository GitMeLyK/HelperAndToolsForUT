using System;
using Microsoft.Extensions.DependencyInjection;
//
using HelperAndToolsForUT.Helper.IocExtensions;
using HelperAndToolsForUT.Helper.IocModule;

namespace HelperAndToolsForUT.Helper.Test.IocModule.BaseTests
{

    public class OuterModule : Module
    {
        protected override void Load(IServiceCollection services)
        {
            base.Load(services);
            services.RegisterModule<InnerModule>();
            services.AddSingleton<IService, ServiceImpl2>();
        }
    }
}
