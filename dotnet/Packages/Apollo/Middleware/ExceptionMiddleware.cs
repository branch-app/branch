using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Apollo.Middleware
{
	public static class ExceptionMiddleware
	{
		public static async Task Handle(HttpContext ctx, Func<Task> next)
		{
			try
			{
				await next.Invoke();
			}
			catch (Exception ex)
			{
				// TODO: Log to sentry

				var json = JsonConvert.SerializeObject(ex);
				ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				ctx.Response.ContentType = "application/json";
				await ctx.Response.WriteAsync(json);
			}
		}
	}
}
