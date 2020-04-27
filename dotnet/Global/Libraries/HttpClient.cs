using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Client = System.Net.Http.HttpClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Crpc.Exceptions;

namespace Branch.Global.Libraries
{
	public class HttpClientOptions
	{
		public HttpClientOptions(Dictionary<string, string> headers = null, TimeSpan? timeout = null)
		{
			Headers = headers ?? new Dictionary<string, string>();
			Timeout = timeout ?? TimeSpan.FromMilliseconds(2000);
		}

		public Dictionary<string, string> Headers { get; set; }

		public TimeSpan Timeout { get; set; }
	}

	public class HttpClient
	{
		// The internal http client shared for every request.
		internal Client Client { get; private set; }

		/// <summary>
		/// Options to be passed into every request.
		/// </summary>
		public HttpClientOptions Options { get; private set; }

		/// <summary>
		/// The base url to be prefixed to every request.
		/// </summary>
		public Uri BaseUrl { get; private set; }

		/// <summary>
		/// Initializes a new HttpClient.
		/// </summary>
		/// <param name="baseUrl">The base url to be prefixed to every request.</param>
		/// <param name="options">Options to be passed into every request.</param>
		/// <param name="client">Client to pass in - only used for tests really.</param>
		public HttpClient(string baseUrl, HttpClientOptions options = null, Client client = null)
		{
			Client = client;

			if (Client == null)
			{
				var handler = new HttpClientHandler
				{
					AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				};

				Client = new Client(handler);
			}

			BaseUrl = new Uri(baseUrl);
			Options = options ?? new HttpClientOptions();

			// Add headers
			foreach (var (key, value) in Options.Headers.Select(h => (h.Key, h.Value)))
				Client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);

			// Add timeout
			Client.Timeout = Options.Timeout;
		}

		public async Task<HttpResponseMessage> Do(string verb, string path, Dictionary<string, string> query = null, HttpClientOptions newOpts = null)
		{
			return await Do(verb, path, query, null, newOpts);
		}

		public async Task<HttpResponseMessage> Do(string verb, string path, Dictionary<string, string> query, HttpContent body, HttpClientOptions newOpts = null)
		{
			if (!Uri.TryCreate(BaseUrl, path, out Uri uri))
				throw new UriFormatException("uri_invalid");

			// Add query
			var queryStr = "";
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
				request.Content = body;

			try
			{
				var response = await Client.SendAsync(request, cts.Token);

				return response;
			}
			catch (OperationCanceledException)
			{
				// Handle timeouts well
				throw new CrpcException(
					"request_timeout",
					new Dictionary<string, object>
					{
						{ "url", uri.ToString() },
						{ "verb", verb },
					}
				);
			}
			catch
			{
				// If any other kind of exception, throw it
				throw;
			}
		}

		private TimeSpan getTimeout(HttpClientOptions options, HttpClientOptions domOpts)
		{
			if (domOpts?.Timeout != null)
				return domOpts.Timeout;

			if (options?.Timeout != null)
				return options.Timeout;

			// Should never happen
			throw new CrpcException("client_has_no_timeout");
		}
	}
}
