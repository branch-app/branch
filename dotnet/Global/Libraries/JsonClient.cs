using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Crpc.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Branch.Global.Libraries
{
	public class JsonClient
	{
		// The internal http client shared for every request.
		public HttpClient Client { get; private set; }

		/// <summary>
		/// The code used when there is a generic http failure.
		/// </summary>
		public readonly string RequestFailedCode = "request_failed";

		private JsonSerializerSettings _jss { get; }

		/// <summary>
		/// Initializes a new JsonClient.
		/// </summary>
		/// <param name="baseUrl">The base url to be prefixed to every request.</param>
		/// <param name="options">Options to be passed into every request.</param>
		/// <param name="client">Client to pass in - only used for tests really.</param>
		public JsonClient(string baseUrl, HttpClientOptions options = null)
		{
			options = options ?? new HttpClientOptions();
			options.Headers.Add("Accept", "application/json");

			Client = new HttpClient(baseUrl, options);

			_jss = new JsonSerializerSettings
			{
				ContractResolver = new DefaultContractResolver
				{
					NamingStrategy = new SnakeCaseNamingStrategy()
				}
			};
		}

		public async Task<TRes> Do<TRes>(string verb, string path, HttpClientOptions newOpts = null)
			where TRes : class
		{
			return await Do<TRes>(verb, path, null, null, newOpts);
		}

		public async Task<TRes> Do<TRes>(string verb, string path, Dictionary<string, string> query = null, HttpClientOptions newOpts = null)
			where TRes : class
		{
			return await Do<TRes>(verb, path, query, null, newOpts);
		}

		public async Task<TRes> Do<TRes>(string verb, string path, object body, HttpClientOptions newOpts = null)
			where TRes : class
		{
			return await Do<TRes>(verb, path, null, body, newOpts);
		}

		public async Task<TRes> Do<TRes>(string verb, string path, Dictionary<string, string> query, object body, HttpClientOptions newOpts = null)
			where TRes : class
		{
			HttpContent content = null;

			if (body != null)
				content = new StringContent(JsonConvert.SerializeObject(body, _jss), Encoding.UTF8, "application/json");

			var response = await Client.Do(verb, path, query, content, newOpts);
			var str = await response.Content.ReadAsStringAsync();

			Console.WriteLine(str);

			if (response.IsSuccessStatusCode)
			{
				if (String.IsNullOrWhiteSpace(str))
					return null;

				return JsonConvert.DeserializeObject<TRes>(str);
			}

			var isJson = response.Content.Headers?.ContentType?.MediaType == "application/json";
			var hasContent = !String.IsNullOrWhiteSpace(str);

			if (isJson && hasContent)
				throwIfCrpcException(str);

			response.EnsureSuccessStatusCode();

			// NOTE(afr): This shouldn't be possible to hit
			throw new InvalidOperationException("Not poss");
		}

		private TimeSpan getTimeout(HttpClientOptions options, HttpClientOptions domOpts)
		{
			if (domOpts?.Timeout != null)
				return domOpts.Timeout;

			if (options?.Timeout != null)
				return options.Timeout;

			// Should never happen
			return TimeSpan.FromSeconds(2);
		}

		private void throwIfCrpcException(string str)
		{
			var err = JsonConvert.DeserializeObject<CrpcException>(str);

			if (err.Message != null)
				throw err;
		}
	}
}
