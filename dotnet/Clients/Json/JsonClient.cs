using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Branch.Clients.Json.Models;
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
		internal HttpClient Client { get; private set; }

		/// <summary>
		/// Options to be passed into every request.
		/// </summary>
		public Options Options { get; private set; }

		/// <summary>
		/// The base url to be prefixed to every request.
		/// </summary>
		public Uri BaseUrl { get; private set; }

		/// <summary>
		/// The code used when there is a generic http failure.
		/// </summary>
		public readonly string RequestFailedCode = "request_failed";

		/// <summary>
		/// The code used when there is an planned failure.
		/// </summary>
		public readonly string PlannedFailureCode = "change_me";

		private JsonSerializerSettings jss;

		/// <summary>
		/// Initializes a new JsonClient.
		/// </summary>
		/// <param name="baseUrl">The base url to be prefixed to every request.</param>
		/// <param name="options">Options to be passed into every request.</param>
		/// <param name="client">Client to pass in - only used for tests really.</param>
		public JsonClient(string baseUrl, Options options = null, HttpClient client = null)
		{
			Client = client;

			if (Client == null)
			{
				var handler = new HttpClientHandler
				{
					AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				};

				Client = new HttpClient(handler);
			}

			BaseUrl = new Uri(baseUrl);
			Options = options ?? new Options();

			// Add headers
			foreach (var (key, value) in Options.Headers.Select(h => (h.Key, h.Value)))
				Client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);

			// Add timeout
			Client.Timeout = Options.Timeout;

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
			if (!Uri.TryCreate(BaseUrl, path, out Uri uri))
				throw new UriFormatException("uri_invalid");

			var queryStr = "";

			// Add query
			if (query != null && query.Any())
				queryStr = "?" + String.Join("&", query.Select(q => $"{q.Key}={q.Value}"));

			// Setup values and timeout
			var request = new HttpRequestMessage(new HttpMethod(verb), uri.ToString() + queryStr);
			var timeout = getTimeout(Options, newOpts);
			var cts = new CancellationTokenSource(timeout);

			// Add headers
			if (newOpts != null && newOpts.Headers != null && newOpts.Headers.Any())
				foreach (var (key, value) in newOpts.Headers.Select(h => (h.Key, h.Value)))
					request.Headers.TryAddWithoutValidation(key, value);

			// Set body
			if (body != null)
			{
				var content = JsonConvert.SerializeObject(body);
				request.Content = new StringContent(content, Encoding.UTF8, "application/json");
			}

			var response = await Client.SendAsync(request, cts.Token);
			var str = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				if (String.IsNullOrWhiteSpace(str))
					return null;

				return JsonConvert.DeserializeObject<TRes>(str);
			}

			var isJson = response.Content.Headers.ContentType.MediaType == "application/json";
			var hasContent = !String.IsNullOrWhiteSpace(str);

			if (isJson && hasContent)
				throwIfBranchException(str);

			throw new BranchException(RequestFailedCode, new Dictionary<string, object>
			{
				{ "url", uri.ToString() },
				{ "verb", verb },
				{ "status_code", response.StatusCode },
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
