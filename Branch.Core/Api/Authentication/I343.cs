using System;
using System.Diagnostics;
using System.Net;
using Branch.Core.Properties;
using Branch.Core.Storage;
using Branch.Models.Authentication;
using EasyHttp.Http;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using Twilio;

namespace Branch.Core.Api.Authentication
{
	/// <summary>
	///     Class for dealing with 343 Industries authentication systems
	/// </summary>
	public static class I343
	{
		/// <summary>
		///     Updates the stored spartan tokens used for authenticating with 343's backend api systems.
		/// </summary>
		/// <returns>A boolean saying if everything was</returns>
		public static bool UpdateAuthentication(AzureStorage storage)
		{
			bool everythingWentGucci;
			var httpClient = new HttpClient();
			var response =
				httpClient.Get(string.Format("http://authentication.xeraxic.com/api/halo4/?email={0}&password={1}",
					CloudConfigurationManager.GetSetting("WlidAuthEmail"),
					CloudConfigurationManager.GetSetting("WlidAuthPassword")));

			if (response.StatusCode == HttpStatusCode.OK && !String.IsNullOrEmpty(response.RawText.Trim()))
			{
				try
				{
					var currentWaypointTokenEntity = storage.Table.RetrieveSingleEntity<WaypointTokenEntity>("Authentication",
						WaypointTokenEntity.FormatRowKey(), storage.Table.AuthenticationCloudTable);

					var waypointToken = JsonConvert.DeserializeObject<WaypointTokenEntity>(response.RawText);

					// save this data to BranchStorage
					if (currentWaypointTokenEntity != null)
					{
						everythingWentGucci = true;

						currentWaypointTokenEntity.ResponseCode = waypointToken.ResponseCode;
						currentWaypointTokenEntity.SpartanToken = waypointToken.SpartanToken;
						currentWaypointTokenEntity.UserInformation = waypointToken.UserInformation;

						storage.Table.ReplaceSingleEntity(currentWaypointTokenEntity, storage.Table.AuthenticationCloudTable);
					}
					else
						everythingWentGucci = storage.Table.InsertSingleEntity(waypointToken, storage.Table.AuthenticationCloudTable);
				}
				catch (JsonReaderException jsonReaderException)
				{
					Trace.TraceError(jsonReaderException.ToString());
					everythingWentGucci = false;
				}
			}
			else
				everythingWentGucci = false;

			if (everythingWentGucci)
				return true;

			// Add custom model
			var customWaypointResponse = new WaypointTokenEntity
			{
				ResponseCode = -1,
				UserInformation = null
			};
			storage.Table.InsertSingleEntity(customWaypointResponse, storage.Table.AuthenticationCloudTable);

			// send sms
			new TwilioRestClient(CloudConfigurationManager.GetSetting("TwilioSid"), CloudConfigurationManager.GetSetting("TwilioAuthToken")).SendSmsMessage(
				CloudConfigurationManager.GetSetting("TwilioFromNumber"), CloudConfigurationManager.GetSetting("TwilioToNumber"),
				"Branch just failed to update it's Halo 4 Authentication Tokens. fuck.");

			return false;
		}
	}
}