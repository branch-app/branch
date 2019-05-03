using System;
using System.Threading.Tasks;
using Branch.Packages.Bae;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sentry;

namespace Branch.Packages.Crpc.Middleware
{
	public sealed class ExceptionMiddleware : IMiddleware
	{
		private readonly ILogger _logger;
		private readonly IHub _sentry;
		private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
		{
			ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
		};

		public ExceptionMiddleware(ILoggerFactory loggerFactory, IHub sentry)
		{
			if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

			_logger = loggerFactory.CreateLogger(nameof(ExceptionMiddleware));
			_sentry = sentry;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				_sentry.CaptureException(ex);

				var bae = ex as BaeException;
				if (!(ex is BaeException))
				{
					// TODO(0xdeafcafe): How should this be done?
					bae = new BaeException(BaeCodes.Unknown);
				}

				var json = JsonConvert.SerializeObject(bae, _jsonSerializerSettings);

				context.Response.StatusCode = bae.StatusCode();
				context.Response.ContentType = "application/json; charset=utf-8";
				await context.Response.WriteAsync(json);
			}
		}
	}
}
