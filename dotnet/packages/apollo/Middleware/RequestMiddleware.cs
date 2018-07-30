using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Apollo.Exceptions;
using Apollo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Schema;

namespace Apollo.Middleware
{
	public static class RequestMiddleware
	{
		private static readonly Regex urlRegex = new Regex(@"/(?<date>\d{4}-\d{2}-\d{2}|latest)/(?<method>[a-z\d_]+)", RegexOptions.Compiled);
		private static MethodInfo jsonDeserializeMethod;
		private static object jsonSerializerInstance;

		static RequestMiddleware()
		{
			var serializerType = typeof(JsonSerializer);

			jsonSerializerInstance = Activator.CreateInstance(serializerType);
			jsonDeserializeMethod = serializerType.GetMethods()
				.Where(i => i.Name == "Deserialize")
				.Where(i => i.IsGenericMethod)
				.Single();
		}

		public static void Handle(IApplicationBuilder app) => app.Run(handleRun);

		private static async Task handleRun(HttpContext ctx)
		{
			// Reject any non POST requests
			if (ctx.Request.Method != "POST")
				throw new ApolloException("method_not_allowed");

			// Check accept header
			checkAccept(ctx);

			// Parse the path
			var match = urlRegex.Match(ctx.Request.Path.ToUriComponent());
			if (match.Groups.Count != 3)
				throw new ApolloException("invalid_url");

			// Pull info out of url
			var date = match.Groups[1].Value;
			var method = match.Groups[2].Value;

			// Check method exists
			if (!ApolloStartup.Methods.TryGetValue(method, out var versions))
				throw new ApolloException("not_found");

			// Get the method we want
			VersionDescriber versionDescriber;
			if (date != "latest")
			{
				if (!versions.TryGetValue(date, out versionDescriber))
					throw new ApolloException("invalid_date");
			}
			else
				versionDescriber = versions.OrderByDescending(v => DateTime.Parse(v.Key)).First().Value;

			// Check content-type and parse body
			var request = readRequest(ctx, versionDescriber);

			// Run method logic
			var response = await (dynamic) versionDescriber.Method.Invoke(null, new object[] { request });

			// Parse response
			await writeResponse(ctx, response);
		}

		private static void checkAccept(HttpContext ctx)
		{
			if (!ctx.Request.Headers.TryGetValue("Accept", out var accept))
				throw new ApolloException("unsupported_accept");

			var allValues = String.Join(",", accept).ToLower();

			if (!allValues.Contains("application/json"))
				throw new ApolloException("unsupported_accept");
		}

		private static object readRequest(HttpContext ctx, VersionDescriber versionDescriber)
		{
			if ((ctx.Request.ContentLength ?? 0) == 0)
				throw new ApolloException("invalid_body");

			using (var sr = new StreamReader(ctx.Request.Body))
			using (var jtr = new JsonTextReader(sr))
			using (var jsv = new JSchemaValidatingReader(jtr))
			{
				var validationErrors = new List<ApolloException>();

				jsv.Schema = versionDescriber.JsonSchema;
				jsv.ValidationEventHandler += (o, a) =>
				{
					validationErrors.Add(new ApolloException("validation_error", new Dictionary<string, object>
					{
						{ "message", a.Message }
					}));
				};

				object deserialized = null;
				try
				{
					deserialized = jsonDeserializeMethod
						.MakeGenericMethod(versionDescriber.RequestType)
						.Invoke(jsonSerializerInstance, new object[] { jsv });
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
					throw new ApolloException("validation_failed", new AggregateException(validationErrors.ToArray()));

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

			var json = JsonConvert.SerializeObject(response);

			ctx.Response.StatusCode = (int)HttpStatusCode.OK;
			ctx.Response.ContentType = "application/json";
			await ctx.Response.WriteAsync(json);
		}
	}
}
