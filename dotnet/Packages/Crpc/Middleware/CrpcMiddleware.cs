using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Branch.Packages.Crpc.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Branch.Packages.Crpc.Middleware
{
	public sealed class CrpcMiddleware : IMiddleware
	{
		private const int _streamBufferSize = 1024;

		private readonly IServiceProvider _services;
		private readonly IHostingEnvironment _environment;
		private readonly ILogger _logger;
		private readonly MethodInfo _jsonDeserializeMethod;
		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private readonly JsonSerializer _jsonSerializer;

		private object _server;
		private CrpcRegistrationOptions _registrationOptions;

		public CrpcMiddleware(IServiceProvider services, IHostingEnvironment environment, ILoggerFactory loggerFactory)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (environment == null) throw new ArgumentNullException(nameof(environment));
			if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

			_services = services;
			_environment = environment;
			_logger = loggerFactory.CreateLogger(nameof(CrpcMiddleware));

			// Do initial reflection setup
			_jsonSerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new DefaultContractResolver
				{
					NamingStrategy = new SnakeCaseNamingStrategy()
				}
			};
			_jsonSerializer = JsonSerializer.Create(_jsonSerializerSettings);
			_jsonDeserializeMethod = typeof(JsonSerializer).GetMethods()
				.Where(i => i.Name == "Deserialize")
				.Where(i => i.IsGenericMethod)
				.Single();
		}

		internal void SetRegistrationOptions(CrpcRegistrationOptions opts)
		{
			if (_registrationOptions != null)
				throw new InvalidOperationException("registration options already set.");

			var svt = opts.ServerType;

			_registrationOptions = opts;
			_server = _services.GetService(svt) ?? ActivatorUtilities.CreateInstance(_services, svt);
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			// _logger.LogCritical("testerman");

			// TODO(0xdeafcafe): Handle request!

			context.Response.StatusCode = (int)HttpStatusCode.OK;
			await context.Response.WriteAsync("ya boi");
		}
	}
}
