using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Branch.Core.Storage;
using Branch.Models.Authentication;
using Branch.Models.Services.Halo4;
using Branch.Models.Services.Halo4._343;
using Branch.Models.Services.Halo4._343.Responses;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using Newtonsoft.Json;

namespace Branch.Core.Api.Halo4
{
	public class WaypointManager
	{
		private const string RegisterWebAppLocation =
			"https://settings.svc.halowaypoint.com/RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752";

		private const string Language = "en-US";
		private const string Game = "h4";
		private const string GameLong = "halo4";
		private readonly AzureStorage _storage;
		private RegisterWebApp _registeredWebApp;

		public Metadata Metadata { get; private set; }

		public WaypointManager(AzureStorage storage)
		{
			_storage = storage;
			Settings.LoadSettings();
			RegisterWebApp();
		}


		#region Setup Waypoint Manager

		public void RegisterWebApp()
		{
			var response = UnauthorizedRequest(RegisterWebAppLocation);

			if (response.StatusCode == HttpStatusCode.OK && !String.IsNullOrEmpty(response.RawText))
			{
				try
				{
					_registeredWebApp = JsonConvert.DeserializeObject<RegisterWebApp>(response.RawText);
				}
				catch (JsonReaderException jsonReaderException)
				{
					Trace.TraceError(jsonReaderException.ToString());
					throw;
				}
			}
			else
			{
				Trace.TraceError("Unable to register web application.");
				throw new HttpException();
			}
		}

		public void GetMetadata()
		{
			
		}

		#endregion
		

		#region Player Endpoints

		/// <summary>
		/// Gets a Players Halo 4 Service Record
		/// </summary>
		/// <param name="gamertag">The players Xbox 360 Gamertag.</param>
		/// <returns>The raw JSON of their Service Record</returns>
		public string GetServiceRecord(string gamertag)
		{


			return null;
		}

		#endregion

		#region Meta Endpoints

		/// <summary>
		/// 
		/// </summary>
		public void UpdateMetadata()
		{
			var metaData =
				ValidateResponseAndGetRawText(
					UnauthorizedRequest(PopulateUrl(UrlFromIds(EndpointType.ServiceList, "GetGameMetadata"))));

			// Save Metadata
			_storage.Blob.UploadBlob(_storage.Blob.H4BlobContainer, BlobContainerPath(BlobType.Other, "metadata"), metaData);

			// Update in Class
			Metadata = ParseText<Metadata>(metaData);
		}

		#endregion

		#region Unauthorized Request

		private HttpResponse UnauthorizedRequest(String url)
		{
// ReSharper disable once IntroduceOptionalParameters.Local
			return UnauthorizedRequest(url, HttpMethod.GET);
		}

		private HttpResponse UnauthorizedRequest(String url, HttpMethod requestType)
		{
			return UnauthorizedRequest(url, requestType, new Dictionary<String, String>());
		}

		private HttpResponse UnauthorizedRequest(String url, HttpMethod requestType, Dictionary<String, String> headers)
		{
			if (headers == null)
				headers = new Dictionary<string, string>();

			var httpClient = new HttpClient();
			httpClient.Request.Accept = "application/json";

			foreach (var header in headers)
				httpClient.Request.AddExtraHeader(header.Key, header.Value);

			switch (requestType)
			{
				case HttpMethod.GET:
					return httpClient.Get(url);

				default:
					throw new ArgumentException();
			}
		}

		#endregion

		#region Authorized Request

		private HttpResponse AuthorizedRequest(String url, AuthType authType)
		{
// ReSharper disable once IntroduceOptionalParameters.Local
			return AuthorizedRequest(url, authType, HttpMethod.GET);
		}

		private HttpResponse AuthorizedRequest(String url, AuthType authType, HttpMethod requestType)
		{
			return AuthorizedRequest(url, authType, requestType, new Dictionary<string, string>());
		}

		private HttpResponse AuthorizedRequest(String url, AuthType authType, HttpMethod requestType,
			Dictionary<String, String> headers)
		{
			if (headers == null)
				headers = new Dictionary<string, string>();

			// get auth
			var auth = _storage.Table.RetrieveSingleEntity<WaypointTokenEntity>("Authentication", WaypointTokenEntity.FormatRowKey(),
				_storage.Table.AuthenticationCloudTable);

			switch (authType)
			{
				case AuthType.Spartan:
					headers.Add("X-343-Authorization-Spartan", auth.SpartanToken);
					break;

				default:
					throw new ArgumentException();
			}

			return UnauthorizedRequest(url, requestType, headers);
		}

		#endregion

		#region Api Helpers

		/// <summary>
		///     Returns a URL from a key and endpoint type.
		/// </summary>
		/// <param name="endpointType">The type of endpoint you need to call (ie; ServiceList)</param>
		/// <param name="key">The key url in that endpoint.</param>
		/// <returns>A string representation of the url.</returns>
		public string UrlFromIds(EndpointType endpointType, string key)
		{
			switch (endpointType)
			{
				case EndpointType.ServiceList:
					return _registeredWebApp.ServiceList[key];

				case EndpointType.Settings:
					return _registeredWebApp.Settings[key];

				default:
					throw new ArgumentException();
			}
		}

		/// <summary>
		///     Populates a url with the default params populated.
		/// </summary>
		/// <param name="url">The url to populate.</param>
		/// <returns>A string representation of the populated url</returns>
		public string PopulateUrl(string url)
		{
			return PopulateUrl(url, new Dictionary<string, string>());
		}

		/// <summary>
		///     Populates a url with the default params populated, and also populates custom params.
		/// </summary>
		/// <param name="url">The url to populate.</param>
		/// <param name="customDefaults">Custom params to populate the url with, auto wrapped in the {} brackets.</param>
		/// <returns>A string representation of the populated url</returns>
		public string PopulateUrl(string url, Dictionary<string, string> customDefaults)
		{
			url = url.Replace("{language}", Language);
			url = url.Replace("{game}", Game);

			if (customDefaults == null)
				throw new ArgumentException("Custom Defaults can't be null");

			return customDefaults.Aggregate(url,
				(current, customDefault) => current.Replace("{" + customDefault.Key + "}", customDefault.Value));
		}

		/// <summary>
		///     Checks is a HttpResponse is valid or not.
		/// </summary>
		/// <param name="response">The HttpResponse</param>
		/// <returns>Boolean representation of the validity of the response.</returns>
		public bool ValidateResponse(HttpResponse response)
		{
			return (response != null && response.StatusCode == HttpStatusCode.OK && !String.IsNullOrEmpty(response.RawText));
		}

		/// <summary>
		///     Checks is a HttpResponse is valid or not, and if not returns the Raw Text.
		/// </summary>
		/// <param name="response">The HttpResponse</param>
		public string ValidateResponseAndGetRawText(HttpResponse response)
		{
			return !ValidateResponse(response) ? null : response.RawText;
		}

		/// <summary>
		///     Checks is a HttpResponse is valid or not, and parses it into a model
		/// </summary>
		/// <param name="modelType">The type of model to parse to.</param>
		/// <param name="response">The HttpResponse we are checking and parsing</param>
		/// <returns>Returns null if the response is not valid, and the parsed model if it is.</returns>
		public TModelType ValidateAndParseResponse<TModelType>(TModelType modelType, HttpResponse response)
			where TModelType : Base
		{
			if (!ValidateResponse(response))
				return null;

			try
			{
				return JsonConvert.DeserializeObject<TModelType>(response.RawText);
			}
			catch (JsonReaderException jsonReaderException)
			{
				Trace.TraceError(jsonReaderException.ToString());
			}

			return null;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public bool CheckApiValidity()
		{
			var auth = _storage.Table.RetrieveSingleEntity<WaypointTokenEntity>("Authentication", WaypointTokenEntity.FormatRowKey(),
				_storage.Table.AuthenticationCloudTable);

			if (auth == null)
				return false;

			return (auth.ResponseCode == 1);
		}

		#endregion

		#region Enums

		public enum AuthType
		{
			Spartan
		}

		public enum EndpointType
		{
			ServiceList,
			Settings
		}

		public enum BlobType
		{
			Other,
			PlayerServiceRecord,
			PlayerGameHistory,
			PlayerGame,
			PlayerCommendation
		}

		#endregion

		#region General Helpers

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TBlam"></typeparam>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public TBlam ParseText<TBlam>(string jsonData)
			where TBlam : WaypointResponse
		{
			if (jsonData == null) return null;

			try
			{
				return JsonConvert.DeserializeObject<TBlam>(jsonData);
			}
			catch (JsonReaderException jsonReaderException)
			{
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blobType"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public string BlobContainerPath(BlobType blobType, string fileName)
		{
			string path;

			switch (blobType)
			{
				case BlobType.Other:
					path = "other";
					break;

				case BlobType.PlayerCommendation:
					path = "player-commendation";
					break;

				case BlobType.PlayerGame:
					path = "player-game";
					break;

				case BlobType.PlayerGameHistory:
					path = "player-game-history";
					break;

				case BlobType.PlayerServiceRecord:
					path = "player-service-record";
					break;

				default:
					throw new ArgumentException("Invalid/Unknown Blob Type");
			}

			return string.Format("{0}/{1}.json", path, fileName);
		}

		#endregion
	}
}
