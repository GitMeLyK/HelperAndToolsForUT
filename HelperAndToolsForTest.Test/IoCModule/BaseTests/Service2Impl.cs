using System;

namespace HelperAndToolsForUT.Helper.Test.IocModule.BaseTests
{
	public class Service2Impl1 : IService2
	{
		private readonly IService _service;

		public Service2Impl1(IService service)
		{
			_service = service;
		}

		public string Message() => _service.Message();
	}
}