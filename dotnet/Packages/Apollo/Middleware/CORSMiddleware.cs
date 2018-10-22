using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Branch.Packages.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Apollo.Middleware
{
	public static class CORSMiddleware
	{
		static CORSMiddleware()
		{
			AllowedOrigins = new StringValues("*");
			AllowedHeaders = new StringValues(new string[] { "Authorization", "Content-Type", "Accept" });
			AllowedMethods = new StringValues("POST");
		}

		/// <summary>
		/// A list of CORS allowed origins.
		/// </summary>
		public readonly static StringValues AllowedOrigins;

		/// <summary>
		/// A list of CORS allowed headers.
		/// </summary>
		public readonly static StringValues AllowedHeaders;

		/// <summary>
		/// A list of CORS allowed methods.
		/// </summary>
		public readonly static StringValues AllowedMethods;

		public static async Task Handle(HttpContext ctx, Func<Task> next)
		{
			ctx.Response.Headers.Add("Access-Control-Allow-Origin", AllowedOrigins);
			ctx.Response.Headers.Add("Access-Control-Allow-Headers", AllowedHeaders);
			ctx.Response.Headers.Add("Access-Control-Allow-Methods", AllowedMethods);

			// If a preflight check, return an empty body
			if (ctx.Request.Method == "OPTIONS")
			{
				ctx.Response.StatusCode = (int) HttpStatusCode.NoContent;
			}
			else
			{
				await next.Invoke();
			}
		}
	}
}
