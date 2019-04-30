using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace Branch.Packages.Crpc
{
	public sealed class CrpcMiddleware : IMiddleware, IDisposable
	{
		private const int _streamBufferSize = 1024;

		private readonly IServiceProvider _services;
		private readonly IHostingEnvironment _environment;
		private readonly ILogger _logger;

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
		}

		internal void SetRegistrationOptions(CrpcRegistrationOptions opts)
		{
			if (_registrationOptions != null)
				throw new InvalidOperationException("registration options already set.");

			var svt = _registrationOptions.ServerType;

			_registrationOptions = opts;
			_server = _services.GetService(svt) ?? ActivatorUtilities.CreateInstance(_services, svt);
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			_logger.LogCritical("testerman");

			await Task.Delay(1);
		}

		public void Dispose()
		{
			// TODO(0xdeafcafe): I don't think this will ever be needed?
			// (_server as IDisposable)?.Dispose();
		}
	}
}
