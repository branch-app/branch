using System;
using Microsoft.Extensions.DependencyInjection;

namespace Branch.Packages.Extensions
{
	public static class ServiceProviderExtensions
	{

		public static T GetOrActivateService<T>(this IServiceProvider serviceProvider)
			where T : class
		{
			return ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, typeof(T)) as T;
		}
	}
}
