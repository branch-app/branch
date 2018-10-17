using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Apollo.Models;
using Branch.Packages.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;

namespace Apollo.Middleware
{
	public static class RequestMiddleware
	{
		private static readonly Regex urlRegex = new Regex(@"/(?<date>\d{4}-\d{2}-\d{2}|latest)/(?<method>[a-z\d_]+)", RegexOptions.Compiled);
		private static MethodInfo jsonDeserializeMethod;
		private static JsonSerializerSettings jsonSerializerSettings;
		private static JsonSerializer jsonSerializer;
		private static object rpcInstance;

		static RequestMiddleware()
		{
			var serializerType = typeof(JsonSerializer);

			jsonSerializerSettings = new JsonSerializerSettings
			{
				ContractResolver = new DefaultContractResolver
				{
					NamingStrategy = new SnakeCaseNamingStrategy()
				}
			};

			jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
			jsonDeserializeMethod = serializerType.GetMethods()
				.Where(i => i.Name == "Deserialize")
				.Where(i => i.IsGenericMethod)
				.Single();
		}

		public static void SetRpc<TRpc>(TRpc rpc) => rpcInstance = rpc;

		public static void Handle<T>(IApplicationBuilder app)
			where T : BaseConfig, new() => app.Run(handleRun<T>);

		private static async Task handleRun<T>(HttpContext ctx)
			where T : BaseConfig, new()
		{
			// Reject any non POST requests
			if (ctx.Request.Method != "POST")
				throw new BranchException("method_not_allowed");

			// Check accept header
			checkAccept(ctx);

			// Parse the path
			var match = urlRegex.Match(ctx.Request.Path.ToUriComponent());
			if (match.Groups.Count != 3)
				throw new BranchException("invalid_url");

			// Pull info out of url
			var date = match.Groups[1].Value;
			var method = match.Groups[2].Value;

			// Check method exists
			if (!ApolloStartup<T>.Methods.TryGetValue(method, out var versions))
				throw new BranchException("not_found");

			// Get the method we want
			VersionDescriber versionDescriber;
			if (date != "latest")
			{
				if (!versions.TryGetValue(date, out versionDescriber))
					throw new BranchException("invalid_date");
			}
			else
				versionDescriber = versions.OrderByDescending(v => DateTime.Parse(v.Key)).First().Value;

			// Check content-type and parse body
			var request = readRequest(ctx, versionDescriber);

			// Run method logic
			var response = await (dynamic) versionDescriber.Method.Invoke(rpcInstance, new object[] { request });

			// Parse response
			await writeResponse(ctx, response);
		}

		private static void checkAccept(HttpContext ctx)
		{
			// If there is no accept, just assume application/json
			if (!ctx.Request.Headers.TryGetValue("Accept", out var accept))
				return;

			// Only allow if asked for json, or any is allowed.
			if (!accept.Contains("application/json") && !accept.Contains("*/*"))
				throw new BranchException("unsupported_accept");
		}

		private static object readRequest(HttpContext ctx, VersionDescriber versionDescriber)
		{
			if (versionDescriber.JsonSchema == null)
				return null;

			if ((ctx.Request.ContentLength ?? 0) == 0)
				throw new BranchException("invalid_body");

			using (var sr = new StreamReader(ctx.Request.Body))
			using (var jtr = new JsonTextReader(sr))
			using (var jsv = new JSchemaValidatingReader(jtr))
			{
				var validationErrors = new List<BranchException>();

				jsv.Schema = versionDescriber.JsonSchema;
				jsv.ValidationEventHandler += (o, a) =>
				{
					validationErrors.Add(new BranchException("validation_error", new Dictionary<string, object>
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
					deserialized = jsonDeserializeMethod
						.MakeGenericMethod(versionDescriber.RequestType)
						.Invoke(jsonSerializer, new object[] { jsv });
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
					throw new BranchException("validation_failed", null, validationErrors.AsEnumerable());

				return deserialized;
			}
		}

		private static async Task writeResponse(HttpContext ctx, object response)
		{
			if (response == null)
			{
				ctx.Response.StatusCode = (int)HttpStatusCode.NoContent;
				return;
			}

			var json = JsonConvert.SerializeObject(response, jsonSerializerSettings);

			ctx.Response.StatusCode = (int)HttpStatusCode.OK;
			ctx.Response.ContentType = "application/json";
			await ctx.Response.WriteAsync(json);
		}
	}
}
