using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Branch.Packages.Crpc.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Branch.Packages.Crpc.Middleware
{
	public sealed class AuthMiddleware : IMiddleware
	{
		private readonly ILogger _logger;
		private Nullable<AuthenticationType> _authenticationType;
		private string[] _internalKeys;

		public AuthMiddleware(ILoggerFactory loggerFactory, IOptions<CrpcOptions> options)
		{
			if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
			if (options == null) throw new ArgumentNullException(nameof(options));

			_logger = loggerFactory.CreateLogger(nameof(AuthMiddleware));
			_internalKeys = options.Value.InternalKeys;
		}

		internal void SetAuthentication(AuthenticationType type)
		{
			if (_authenticationType.HasValue)
				throw new InvalidOperationException("authentication type already set");

			_authenticationType = type;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			if (!_authenticationType.HasValue)
				throw new InvalidOperationException("authentication type not set");

			switch(_authenticationType.Value)
			{
				case AuthenticationType.UnsafeNoAuthentication:
					break;

				case AuthenticationType.AllowInternalAuthentication:
					var hasHeader = context.Request.Headers.TryGetValue("Authorization", out var headers);

					if (!hasHeader)
						throw new Exception("unauthorized"); // TODO(0xdeafcafe): Write Cher

					var header = headers[0];

					if (!_internalKeys.Any(k => $"bearer {k}" == header))
						throw new Exception("unauthorized"); // TODO(0xdeafcafe): Write Cher
					break;

				default:
					throw new InvalidOperationException("unknown authentication type");
			}

			await next.Invoke(context);
		}
	}
}
