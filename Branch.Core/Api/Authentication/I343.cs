using System;
using System.Diagnostics;
using System.Net;
using Branch.Core.Storage;
using Branch.Models.Authentication;
using EasyHttp.Http;
using Newtonsoft.Json;
using Twilio;

namespace Branch.Core.Api.Authentication
{
	/// <summary>
	/// Class for dealing with 343 Industries authentication systems
	/// </summary>
	public static class I343
	{
		/// <summary>
		/// Updates the stored spartan tokens used for authenticating with 343's backend api systems.
		/// </summary>
		/// <returns>A boolean saying if everything was</returns>
		public static bool UpdateAuthentication(TableStorage storage)
		{
			Settings.LoadSettings();

			bool everythingWentGucci;
			var httpClient = new HttpClient();
			var response =
				httpClient.Get(string.Format("http://authentication.xeraxic.com/api/halo4/?email={0}&password={1}",
					Settings.WlidAuthEmail,
					Settings.WlidAuthPassword));

			if (response.StatusCode == HttpStatusCode.OK && !String.IsNullOrEmpty(response.RawText.Trim()))
			{
				try
				{
					var currentWaypointTokenEntity = storage.RetrieveSingleEntity<WaypointTokenEntity>("Authentication",
						WaypointTokenEntity.FormatRowKey(), storage.AuthenticationCloudTable);

					var waypointToken = JsonConvert.DeserializeObject<WaypointTokenEntity>(response.RawText);

					// save this data to BranchStorage
					if (currentWaypointTokenEntity != null)
					{
						everythingWentGucci = true;

						currentWaypointTokenEntity.ResponseCode = waypointToken.ResponseCode;
						currentWaypointTokenEntity.SpartanToken = waypointToken.SpartanToken;
						currentWaypointTokenEntity.UserInformation = waypointToken.UserInformation;

						storage.UpdateEntity(currentWaypointTokenEntity, storage.AuthenticationCloudTable);
					}
					else
						everythingWentGucci = storage.InsertSingleEntity(waypointToken, storage.AuthenticationCloudTable);
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
			storage.InsertSingleEntity(customWaypointResponse, storage.AuthenticationCloudTable);

			// send sms
			new TwilioRestClient(Settings.TwilioSid, Settings.TwilioAuthToken).SendSmsMessage(
				Settings.TwilioFromNumber, Settings.TwilioToNumber, "Branch just failed to update it's Halo 4 Authentication Tokens. fuck.");
			return false;
		}
	}
}
