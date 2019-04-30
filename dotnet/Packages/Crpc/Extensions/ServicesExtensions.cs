using System;
using System.Reflection;

using Branch.Packages.Crpc;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServicesExtensions
	{
		public static IServiceCollection AddCrpc(this IServiceCollection services, Action<CrpcOptions> configureOptions)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

			services.Configure(configureOptions);
			services.AddSingleton<CrpcMiddleware>();
			services.AddMvcCore();

			return services;
		}
	}
}
