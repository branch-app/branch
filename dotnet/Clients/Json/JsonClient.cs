using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Branch.Clients.Http;
using Branch.Clients.Http.Models;
using Branch.Packages.Converters;
using Branch.Packages.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: InternalsVisibleTo("Branch.Tests.Clients.JsonTests")]
namespace Branch.Clients.Json
{
	public class JsonClient
	{
		// The internal http client shared for every request.
		public HttpClient Client { get; private set; }

		/// <summary>
		/// The code used when there is a generic http failure.
		/// </summary>
		public readonly string RequestFailedCode = "request_failed";

		/// <summary>
		/// Serializer used to handle JSON on the way out.
		/// </summary>
		private JsonSerializerSettings jss;

		/// <summary>
		/// Initializes a new JsonClient.
		/// </summary>
		/// <param name="baseUrl">The base url to be prefixed to every request.</param>
		/// <param name="options">Options to be passed into every request.</param>
		/// <param name="client">Client to pass in - only used for tests really.</param>
		public JsonClient(string baseUrl, Options options = null)
		{
			options = options ?? new Options();
			options.Headers.Add("Content-Type", "application/json");
			options.Headers.Add("Accept", "application/json");

			Client = new HttpClient(baseUrl, options);

			// Create serializer settings
			jss = new JsonSerializerSettings
			{
				Converters = new List<JsonConverter> { new ExceptionConverter() },
			};
		}

		public async Task<TRes> Do<TRes, TErr>(string verb, string path, Dictionary<string, string> query = null, Options newOpts = null)
			where TRes : class
			where TErr : class
		{
			return await Do<object, TRes, TErr>(verb, path, query, null, newOpts);
		}

		public async Task<TRes> Do<TReq, TRes, TErr>(string verb, string path, Dictionary<string, string> query, TReq body, Options newOpts = null)
			where TReq : class
			where TRes : class
			where TErr : class
		{
			var content = body != null ? JsonConvert.SerializeObject(body) : null;
			var output = await Client.Do(verb, path, query, content, newOpts);
			var str = await output.resp.Content.ReadAsStringAsync();

			if (output.resp.IsSuccessStatusCode)
			{
				if (String.IsNullOrWhiteSpace(str))
					return null;

				return JsonConvert.DeserializeObject<TRes>(str);
			}

			var isJson = output.resp.Content.Headers.ContentType.MediaType == "application/json";
			var hasContent = !String.IsNullOrWhiteSpace(str);

			if (isJson && hasContent)
				throwIfBranchException(str);

			throw new BranchException(RequestFailedCode, new Dictionary<string, object>
			{
				{ "Url", output.req.RequestUri.ToString() },
				{ "Verb", verb },
				{ "StatusCode", output.resp.StatusCode },
			});
		}

		private TimeSpan getTimeout(Options options, Options domOpts)
		{
			if (domOpts?.Timeout != null)
				return domOpts.Timeout;

			if (options?.Timeout != null)
				return options.Timeout;

			// Should never happen
			return TimeSpan.FromSeconds(2);
		}

		private void throwIfBranchException(string str)
		{
			try
			{
				var err = JsonConvert.DeserializeObject<BranchException>(str, jss);

				if (err.Message != null)
					throw err;
			} catch { /* */ }
		}
	}
}
