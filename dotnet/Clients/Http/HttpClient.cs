using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Client = System.Net.Http.HttpClient;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Branch.Clients.Http.Models;
using System.Net.Http;
using Branch.Packages.Exceptions;

[assembly: InternalsVisibleTo("Branch.Tests.Clients.HttpTests")]
namespace Branch.Clients.Http
{
	public class HttpClient
	{
		// The internal http client shared for every request.
		internal Client Client { get; private set; }

		/// <summary>
		/// Options to be passed into every request.
		/// </summary>
		public Options Options { get; private set; }

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
		public HttpClient(string baseUrl, Options options = null, Client client = null)
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
			Options = options ?? new Options();

			// Add headers
			foreach (var (key, value) in Options.Headers.Select(h => (h.Key, h.Value)))
				Client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);

			// Add timeout
			Client.Timeout = Options.Timeout;
		}

		public async Task<(HttpRequestMessage req, HttpResponseMessage resp)> Do(string verb, string path, Dictionary<string, string> query = null, Options newOpts = null)
		{
			return await Do(verb, path, query, null, newOpts);
		}

		public async Task<(HttpRequestMessage req, HttpResponseMessage resp)> Do(string verb, string path, Dictionary<string, string> query, string body, Options newOpts = null)
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
				request.Content = new StringContent(body, Encoding.UTF8);

			try
			{
				var response = await Client.SendAsync(request, cts.Token);

				return (request, response);
			}
			catch (OperationCanceledException)
			{
				// Handle timeouts well
				throw new BranchException(
					"request_timeout",
					new Dictionary<string, object>
					{
						{ "Url", uri.ToString() },
						{ "Verb", verb },
					}
				);
			}
			catch
			{
				// If any other kind of exception, throw it
				throw;
			}
		}

		private TimeSpan getTimeout(Options options, Options domOpts)
		{
			if (domOpts?.Timeout != null)
				return domOpts.Timeout;

			if (options?.Timeout != null)
				return options.Timeout;

			// Should never happen
			throw new BranchException("client_has_no_timeout");
		}
	}
}
