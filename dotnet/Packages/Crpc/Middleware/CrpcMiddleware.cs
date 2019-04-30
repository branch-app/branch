using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Branch.Packages.Bae;
using Branch.Packages.Crpc.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;

namespace Branch.Packages.Crpc.Middleware
{
	public sealed class CrpcMiddleware : IMiddleware
	{
		private readonly Regex _urlRegex = new Regex(@"^/(?<date>\d{4}-\d{2}-\d{2}|latest|preview)/(?<method>[a-z\d_]+)$", RegexOptions.Compiled);

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
			if (context.Request.Method != "POST")
				throw new BaeException(BaeCodes.MethodNotAllowed);

			ensureAcceptIsAllowed(context);

			var match = _urlRegex.Match(context.Request.Path.ToUriComponent());
			if (match.Groups.Count != 3)
				throw new BaeException(BaeCodes.RouteNotFound);

			var urlDate = match.Groups[1].Value;
			var urlMethod = match.Groups[2].Value;

			if (!_registrationOptions.Registrations.ContainsKey(urlMethod))
				throw new BaeException(BaeCodes.RouteNotFound);

			var registration = _registrationOptions.Registrations[urlMethod];
			KeyValuePair<string, CrpcVersionRegistration> version;

			switch(urlDate)
			{
				case "preview":
					version = registration.FirstOrDefault(r => r.Value.IsPreview);
					break;

				case "latest":
					var versions = registration.Where(r => !r.Value.IsPreview).Select(r => r.Key);

					// If the method is only in preview, it should only be exposed if
					// preview is explicitly stated
					if (versions.Count() == 0)
						throw new BaeException(BaeCodes.RouteNotFound);

					var latestVersion = versions.OrderByDescending(v => DateTime.Parse(v)).First();

					version = registration.First(r => r.Key == latestVersion);
					break;

				default:
					if (!registration.ContainsKey(urlDate))
						throw new BaeException(BaeCodes.RouteNotFound);

					version = registration.First(r => r.Key == urlDate);
					break;
			}

			var request = readRequest(context, version.Value);
			var response = await (dynamic) version.Value.Method.Invoke(_server, new object[] { request });

			if (response == null)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NoContent;
				return;
			}

			string json = JsonConvert.SerializeObject(response, _jsonSerializerSettings);
			context.Response.StatusCode = (int)HttpStatusCode.OK;
			context.Response.ContentType = "application/json; charset=utf-8";
			await context.Response.WriteAsync(json);
		}

		private void ensureAcceptIsAllowed(HttpContext context)
		{
			if (!context.Request.Headers.TryGetValue("Accept", out var accept))
				return;

			if (!accept.Contains("application/json") && !accept.Contains("*/*"))
				throw new BaeException(BaeCodes.UnsupportedAccept);
		}

		private object readRequest(HttpContext context, CrpcVersionRegistration version)
		{
			if (version.Schema == null)
				return null;

			if ((context.Request.ContentLength ?? 0) == 0)
				throw new BaeException("invalid_body");

			using (var sr = new StreamReader(context.Request.Body))
			using (var jtr = new JsonTextReader(sr))
			using (var jsv = new JSchemaValidatingReader(jtr))
			{
				var validationErrors = new List<BaeException>();

				jsv.Schema = version.Schema;
				jsv.ValidationEventHandler += (o, a) =>
				{
					validationErrors.Add(new BaeException("validation_error", new Dictionary<string, object>
					{
						{ "message", a.ValidationError.Message },
						{ "value", a.ValidationError.Value },
						{ "path", a.ValidationError.Path },
						{ "location", $"line {a.ValidationError.LineNumber}, position {a.ValidationError.LinePosition}" },
					}));
				};

				object deserialized = null;
				try
				{
					deserialized = _jsonDeserializeMethod
						.MakeGenericMethod(version.RequestType)
						.Invoke(_jsonSerializer, new object[] { jsv });
				}
				catch (TargetInvocationException ex)
				{
					var innerEx = ex.InnerException;

					// If the exception isn't a serialization exception, throw
					if (!(innerEx is JsonSerializationException))
						throw innerEx;
				}

				// Handle errors
				if (validationErrors.Count() > 0)
					throw new BaeException(BaeCodes.ValidationFailed, null, validationErrors.AsEnumerable());

				return deserialized;
			}
		}
	}
}
