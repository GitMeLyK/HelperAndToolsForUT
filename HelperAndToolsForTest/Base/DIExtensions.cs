using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace HelperAndToolsForUT.Helper.Extensions.IocExtensions
{

	/// <summary>
	///		A helper method to register the type as providing all of its public interfaces. 
	/// </summary>
	public static class DiExtensions
	{

		/// <summary>
		///		A helper method to register the type as providing all of its public interfaces. 
		///		This helper methods is manually to use in context also it is possible use a small 
		///		NuGet library (e.g. NetCore.AutoRegisterDi).
		/// </summary>
		/// <remarks>
		///		If you don't use Dependency Management Containers like AutoFac DryOc or whatever, 
		///		and only the built-in .net core IoC is used (Microsoft.Extensions.DependencyInjection).
		/// </remarks>
		/// <typeparam name="TService"></typeparam>
		/// <param name="services"></param>
		/// <param name="lifetime"></param>
		public static void RegisterAsImplementedInterfaces<TService>(this IServiceCollection services, ServiceLifetime lifetime)
		{
			// Note.: https://alex-klaus.com/webapi-proj-without-autofac/ 

			var interfaces = typeof(TService).GetTypeInfo().ImplementedInterfaces
											.Where(i => i != typeof(IDisposable) && (i.IsPublic));

			foreach (Type interfaceType in interfaces)
				services.Add(new ServiceDescriptor(interfaceType, typeof(TService), lifetime));
		}
	}

}
