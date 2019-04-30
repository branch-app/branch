using System;
using System.Reflection;
using Branch.Packages.Crpc;
using Branch.Packages.Crpc.Registration;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
	public static class BuilderExtensions
	{
		public static IApplicationBuilder UseCrpc(this IApplicationBuilder app, PathString baseUrl, Action<CrpcRegistrationOptions> opts)
		{
			if (app == null) throw new ArgumentNullException(nameof(app));
			if (opts == null) throw new ArgumentNullException(nameof(opts));

			var options = new CrpcRegistrationOptions();
			opts.Invoke(options);

			var middleware = app.ApplicationServices.GetService(typeof(CrpcMiddleware)) as CrpcMiddleware;
			middleware.SetRegistrationOptions(options);

			app.Map("/system/health", builder => {
				builder.Run(async context => {
					context.Response.StatusCode = 204;
				});
			});

			app.Map(baseUrl, b => b.UseMiddleware<CrpcMiddleware>());

			return app;
		}
	}
}
