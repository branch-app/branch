using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Branch.Core.Storage;
using HttpMethod = Branch.Core.Enums.HttpMethod;

namespace Branch.Core.Game.HaloReach.Api
{
	public class Manager
	{
		private const string Language = "en-US";
		private const string Game = "h4";
		private const string ApiKey = "waypoint";
		private readonly AzureStorage _storage;

		public Manager(AzureStorage storage)
		{
			_storage = storage;
		}



		#region Unauthorized Request

		private static HttpResponseMessage UnauthorizedRequest(String url)
		{
			// ReSharper disable once IntroduceOptionalParameters.Local
			return UnauthorizedRequest(url, HttpMethod.Get);
		}

		private static HttpResponseMessage UnauthorizedRequest(String url, HttpMethod requestType)
		{
			return UnauthorizedRequest(url, requestType, new Dictionary<String, String>());
		}

		private static HttpResponseMessage UnauthorizedRequest(String url, HttpMethod requestType, Dictionary<String, String> headers)
		{
			if (headers == null)
				headers = new Dictionary<string, string>();

			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

			foreach (var header in headers)
				httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
			switch (requestType)
			{
				case HttpMethod.Get:
					return Task.Run(() => httpClient.GetAsync(url)).Result;

				default:
					throw new ArgumentException();
			}
		}

		#endregion
	}
}
