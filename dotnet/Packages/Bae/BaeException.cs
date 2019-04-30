using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Branch.Packages.Bae
{
	using Meta = Dictionary<string, object>;

	[JsonConverter(typeof(BaeExceptionConverter))]
	public class BaeException : AggregateException
	{
		public BaeException() { }

		public BaeException(string code) : base(code) { }

		public BaeException(string code, Meta data)
			: base(code)
		{
			if (data == null)
				return;

			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}

		public BaeException(string code, Meta data, Exception ex)
			: base(code, ex)
		{
			if (data == null)
				return;

			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}

		public BaeException(string code, Meta data, IEnumerable<Exception> exs)
			: base(code, exs)
		{
			if (data == null)
				return;

			foreach (var pair in data)
				Data.Add(pair.Key, pair.Value);
		}

		public HttpStatusCode StatusCode()
		{
			switch(Message)
			{
				case BaeCodes.Unauthorized:
					return HttpStatusCode.Unauthorized;

				case BaeCodes.AccessDenied:
					return HttpStatusCode.Forbidden;

				case BaeCodes.RouteNotFound:
				case BaeCodes.NotFound:
					return HttpStatusCode.NotFound;

				case BaeCodes.MethodNotAllowed:
					return HttpStatusCode.MethodNotAllowed;

				case BaeCodes.NoLongerSupported:
					return HttpStatusCode.Gone;

				case BaeCodes.TooManyRequests:
					return HttpStatusCode.TooManyRequests;

				case BaeCodes.CoercionError:
				case BaeCodes.Unknown:
					return HttpStatusCode.InternalServerError;

				case BaeCodes.BadRequest:
				default:
					return HttpStatusCode.BadRequest;
			}
		}
	}
}
