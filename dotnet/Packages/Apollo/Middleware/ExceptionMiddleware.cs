using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Apollo.Converters;
using Branch.Packages.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Apollo.Middleware
{
	public static class ExceptionMiddleware
	{
		private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
		{
			Converters = new List<JsonConverter> { new ExceptionConverter() },
			ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
		};

		public static async Task Handle(HttpContext ctx, Func<Task> next)
		{
			try
			{
				await next.Invoke();
			}
			catch (Exception ex)
			{
				// TODO(0xdeafcafe): Handle this
				if (!(ex is BranchException))
				{
					Console.WriteLine(ex);

					throw;
				}

				var json = JsonConvert.SerializeObject(ex, jsonSerializerSettings);

				ctx.Response.StatusCode = (int) HttpStatusCode.InternalServerError;;
				ctx.Response.ContentType = "application/json";
				await ctx.Response.WriteAsync(json);
			}
		}
	}
}
