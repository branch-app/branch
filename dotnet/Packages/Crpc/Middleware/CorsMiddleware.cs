using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Branch.Packages.Crpc.Middleware
{
	public sealed class CorsMiddleware : IMiddleware
	{
		static CorsMiddleware()
		{
			AllowedOrigins = new StringValues("*");
			AllowedHeaders = new StringValues(new string[] { "Authorization", "Content-Type", "Accept" });
			AllowedMethods = new StringValues("POST");
		}

		/// <summary>
		/// A list of CORS allowed origins.
		/// </summary>
		public readonly static StringValues AllowedOrigins;

		/// <summary>
		/// A list of CORS allowed headers.
		/// </summary>
		public readonly static StringValues AllowedHeaders;

		/// <summary>
		/// A list of CORS allowed methods.
		/// </summary>
		public readonly static StringValues AllowedMethods;

		/// <summary>
		/// The logger for the middleware
		/// </summary>
		private readonly ILogger _logger;

		public CorsMiddleware(ILoggerFactory loggerFactory)
		{
			if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

			_logger = loggerFactory.CreateLogger(nameof(CorsMiddleware));
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			context.Response.Headers.Add("Access-Control-Allow-Origin", AllowedOrigins);
			context.Response.Headers.Add("Access-Control-Allow-Headers", AllowedHeaders);
			context.Response.Headers.Add("Access-Control-Allow-Methods", AllowedMethods);

			if (context.Request.Method == "OPTIONS")
				context.Response.StatusCode = (int)HttpStatusCode.NoContent;
			else
				await next.Invoke(context);
		}
	}
}
