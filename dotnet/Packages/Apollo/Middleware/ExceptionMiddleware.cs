using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Branch.Packages.Converters;
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
				var branchEx = ex as BranchException;

				// TODO(0xdeafcafe): Handle this properly
				if (!(ex is BranchException))
				{
					Console.WriteLine(ex);

					branchEx = new BranchException("unknown_error");
				}

				var json = JsonConvert.SerializeObject(branchEx, jsonSerializerSettings);

				ctx.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
				ctx.Response.ContentType = "application/json";
				await ctx.Response.WriteAsync(json);
			}
		}
	}
}
