using System;
using System;
using System.Linq;
using HelperAndToolsForUT.Helper.IocModule;
//
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace HelperAndToolsForUT.Helper.Test.IocModule.BaseTests
{
    public class TestModule : Module
    {
        protected override void Load(IServiceCollection services)
        {
            base.Load(services);
            var _ = (services.FirstOrDefault(x => x.ServiceType == typeof(ILoggerFactory)) ?? throw new ApplicationException());
        }
    }
}
