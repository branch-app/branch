using System;
using System.Net;
using System.Reflection;
using Branch.Packages.Crpc;
using Branch.Packages.Crpc.Middleware;
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

			setupMiddlewares(app, options);

			app.Map("/system/health", builder => {
				builder.Run(async context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.NoContent;
				});
			});

			app.Map(baseUrl, builder =>
			{
				builder.UseMiddleware<CorsMiddleware>();
				builder.UseMiddleware<AuthMiddleware>();
				builder.UseMiddleware<CrpcMiddleware>();
			});

			return app;
		}

		private static void setupMiddlewares(IApplicationBuilder app, CrpcRegistrationOptions options)
		{
			// Set the registration options on the CrpcMiddleware singleton
			var crpcMiddleware = app.ApplicationServices.GetService(typeof(CrpcMiddleware)) as CrpcMiddleware;
			crpcMiddleware.SetRegistrationOptions(options);

			// Set the authentication type on the AuthMiddleware singleton
			var authMiddleware = app.ApplicationServices.GetService(typeof(AuthMiddleware)) as AuthMiddleware;
			authMiddleware.SetAuthentication(options.Authentication);
		}
	}
}
