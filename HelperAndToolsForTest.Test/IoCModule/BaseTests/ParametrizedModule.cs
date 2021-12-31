using System;
using HelperAndToolsForUT.Helper.Abstraction.IOC;
using Microsoft.Extensions.DependencyInjection;


namespace HelperAndToolsForUT.Helper.Test.IocModule.BaseTests
{
	public class ParameterizedModule : Module
	{
		private readonly IService _serviceImpl;

		public ParameterizedModule(IService serviceImpl)
		{
			_serviceImpl = serviceImpl;
		}

		protected override void Load(IServiceCollection services)
		{
			base.Load(services);
			services.AddSingleton<IService2>(new Service2Impl1(_serviceImpl));
		}
	}
}
