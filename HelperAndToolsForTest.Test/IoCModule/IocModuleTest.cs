using System;
using System.Linq;
using System.Collections.Generic;
//using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
//
using FluentAssertions;
using NUnit.Framework;
//
using HelperAndToolsForUT.Helper.IocExtensions;
using HelperAndToolsForUT.Helper.Test.IocModule.BaseTests;

namespace HelperAndToolsForUT.Helper.Test.IocModule
{

    public interface IService{        
        string Message();
    }

    public interface IService2
    {
        string Message();
    }

    [TestFixture]
    public class ModuleExtensionsTests
    {
        [Test]
        public void RegisterModule_WithNestedModules_AllDependenciesAreRegistered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.RegisterModule<OuterModule>();

            using var scope1 = serviceCollection.BuildServiceProvider();
            var services = scope1.GetRequiredService<IEnumerable<IService>>();
            services.Count().Should().Be(2);
        }

        [Test]
        public void RegisterModule_WhenAFactoryFunctionIsInvoked_AModuleIsRegistered()
        {
            var serviceCollection = new ServiceCollection();
            if (new Random().Next() % 2 == 0)
            {
                serviceCollection.AddSingleton<IService, ServiceImpl2>();
            }
            else
            {
                serviceCollection.AddSingleton<IService, ServiceImpl1>();
            }

            serviceCollection.RegisterModule((collection) =>
            {
                var impl = collection.BuildServiceProvider().GetRequiredService<IService>();
                return new ParameterizedModule(impl);
            });

            using var scope1 = serviceCollection.BuildServiceProvider();
            var service = scope1.GetRequiredService<IService2>();
            service.Message().Should().Be(scope1.GetRequiredService<IService>().Message());
        }

        [Test]
        public void RegisterModule_WhenCalled_PreviousRegistrationsArePresent()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(c =>
            {
                c.ClearProviders();
                c.SetMinimumLevel(LogLevel.Debug);
                //c.AddNLog();
            });
            Action act = () => serviceCollection.RegisterModule<TestModule>();
            act.Should().NotThrow();
        }
    }

}
