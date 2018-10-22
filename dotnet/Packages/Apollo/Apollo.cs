using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Apollo.Models;
using Branch.Packages.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Schema;
using System.IO;
using static Ksuid.Ksuid;
using Apollo.Middleware;
using Apollo.Configuration;
using Microsoft.Extensions.Options;

namespace Apollo
{
	/// <summary>
	/// This handles the initial startup logic for a service that implements Apollo.
	/// </summary>
	/// <typeparam name="T">The object that contains the configuration</typeparam>
	public class ApolloStartup<T>
		where T : BaseConfig, new()
	{
		internal static readonly Regex EndpointRegex = new Regex(@"[a-z]{1}[a-z0-9_]+", RegexOptions.Compiled);

		internal static Dictionary<string, Dictionary<string, VersionDescriber>> Methods { get; set; }
			= new Dictionary<string, Dictionary<string, VersionDescriber>>();

		public string ServiceName { get; private set; }

		public T Configuration { get; private set; }

		public IHostingEnvironment HostingEnvironment { get; private set; }

		public ApolloStartup(IHostingEnvironment environment, string serviceName)
		{
			ServiceName = serviceName;
			HostingEnvironment = environment;
			var builtConfig = ApolloConfiguration.Add(environment);

			if (environment.IsDevelopment())
				Ksuid.Ksuid.Environment = "dev";
			else if (!environment.IsProduction())
				Ksuid.Ksuid.Environment = environment.EnvironmentName;

			Configuration = new T();
			ConfigurationBinder.Bind(builtConfig, Configuration);
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore();
		}

		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// Exception handling middleware
			app.Use(ExceptionMiddleware.Handle);

			// Set CORS headers
			app.Use(CORSMiddleware.Handle);

			// Add health check
			app.Map("/system/health", innerApp => {
				innerApp.Run(async context => {
					context.Response.StatusCode = 204;

					await context.Response.WriteAsync("");
				});
			});

			// Check authorization header
			app.Use((HttpContext ctx, Func<Task> next) =>
				AuthorizationMiddleware.Handle(Configuration.InternalKeys, ctx, next));

			// Custom routing
			app.Map("/1", RequestMiddleware.Handle<T>);
		}

		public void RpcRegistration<TRpc>(TRpc rpc) => RequestMiddleware.SetRpc(rpc);

		public void RegisterMethod<TReq, TRes>(string endpoint, string date, Func<TReq, Task<TRes>> method, string jsonSchema)
		{
			// Validate date format
			DateTime.ParseExact(date, "yyyy-MM-dd", null);

			// Validate endpoint
			if (!EndpointRegex.Match(endpoint).Success)
				throw new BranchException("endpoint_format_invalid", new Dictionary<string, object> { { "Endpoint", endpoint } });

			// If the endpoint is fresh, create the dictionary
			if (!Methods.TryGetValue(endpoint, out var versions))
				versions = new Dictionary<string, VersionDescriber>();

			// Check if the endpoint already has this date specified
			if (versions.ContainsKey(date))
				throw new BranchException(
					"An endpoint with that date has already been registered.",
					new Dictionary<string, object> {
						{ "Endpoint", endpoint },
						{ "Date", date },
					}
				);

			// Use reflection to get stuff
			var methodInfo = method.Method;

			// Check param counts
			if (methodInfo.GetParameters().Length != 1)
				throw new BranchException("invalid_param_length", new Dictionary<string, object> { { "MethodName", methodInfo.Name } });

			if (methodInfo.ReturnType?.GenericTypeArguments.Length != 1)
				throw new BranchException("invalid_return_argument_length", new Dictionary<string, object> { { "MethodName", methodInfo.Name } });

			// Get request and response types
			var requestType = methodInfo.GetParameters()[0].ParameterType;
			var responseType = methodInfo.ReturnType.GenericTypeArguments[0];

			// Parse JsonSchema
			var schema = JSchema.Parse(jsonSchema);

			// Add method to version
			versions.Add(date, new VersionDescriber
			{
				Method = methodInfo,
				RequestType = requestType,
				ResponseType = responseType,
				JsonSchema = schema,
			});

			// Update versions in methods
			Methods[endpoint] = versions;
		}

		public class EmptyRequest { }
	}
}
