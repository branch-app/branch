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

		public int StatusCode()
		{
			switch(Message)
			{
				case BaeCodes.Unauthorized:
					return (int) HttpStatusCode.Unauthorized;

				case BaeCodes.AccessDenied:
					return (int) HttpStatusCode.Forbidden;

				case BaeCodes.RouteNotFound:
				case BaeCodes.NotFound:
					return (int) HttpStatusCode.NotFound;

				case BaeCodes.MethodNotAllowed:
					return (int) HttpStatusCode.MethodNotAllowed;

				case BaeCodes.NoLongerSupported:
					return (int) HttpStatusCode.Gone;

				case BaeCodes.ValidationFailed:
					return 422;

				case BaeCodes.TooManyRequests:
					return 429;

				case BaeCodes.CoercionError:
				case BaeCodes.Unknown:
					return (int) HttpStatusCode.InternalServerError;

				case BaeCodes.BadRequest:
				case BaeCodes.UnsupportedAccept:
				default:
					return (int) HttpStatusCode.BadRequest;
			}
		}
	}
}
