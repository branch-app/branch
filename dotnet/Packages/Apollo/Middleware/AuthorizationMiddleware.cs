using System;
using System.Collections.Generic;
using System.Linq;
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
	public static class AuthorizationMiddleware
	{
		public static async Task Handle(string[] keys, HttpContext ctx, Func<Task> next)
		{
			var hasHeader = ctx.Request.Headers.TryGetValue("Authorization", out var headers);

			if (!hasHeader)
				throw new BranchException("invalid_authentication");

			var header = headers[0];

			if (!keys.Any(k => $"bearer {k}" == header))
				throw new BranchException("invalid_authentication");

			await next.Invoke();
		}
	}
}
