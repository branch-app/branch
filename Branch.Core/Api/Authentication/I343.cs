using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Branch.Core.Storage;
using Branch.Models.Authentication;
using EasyHttp.Http;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using SendGrid;

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
			var everythingWentGucci = false;
			var strResponse = "";
			var httpClient = new HttpClient();

			// Try up to 10 times
			for (var i = 0; i < 10; i++)
			{
				var response = httpClient.Get(CloudConfigurationManager.GetSetting("SpartanTokenApi"));

				if (response.StatusCode == HttpStatusCode.OK && !String.IsNullOrEmpty(response.RawText.Trim()))
				{
					try
					{
						strResponse = response.RawText;
						var currentWaypointTokenEntity = storage.Table.RetrieveSingleEntity<WaypointTokenEntity>("Authentication",
							WaypointTokenEntity.FormatRowKey(), storage.Table.AuthenticationCloudTable);

						var waypointToken = JsonConvert.DeserializeObject<WaypointTokenEntity>(response.RawText);

						if (waypointToken != null)
							if (currentWaypointTokenEntity != null)
							{
								everythingWentGucci = true;
								currentWaypointTokenEntity.SpartanToken = waypointToken.SpartanToken;
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

				if (everythingWentGucci)
					break;
			}

			if (everythingWentGucci)
				return true;

			// Add custom model
			var customWaypointResponse = new WaypointTokenEntity
			{
				SpartanToken = null
			};
			storage.Table.InsertOrReplaceSingleEntity(customWaypointResponse, storage.Table.AuthenticationCloudTable);

			// send glorious email!
			var text =
				String.Format(
					"Dear Self, {0}Halo 4's authenication failed to update. Might want to look into it. Below is the response the server recieved from the auth service: {0}{0}{1}{0}{0}Best Regards,{0}Branch",
					Environment.NewLine, strResponse);

			new Web(new NetworkCredential(CloudConfigurationManager.GetSetting("SendGridUser"),
				CloudConfigurationManager.GetSetting("SendGridPass"))).Deliver(
					new SendGridMessage(new MailAddress("info@branchapp.co"),
						new[] {new MailAddress(CloudConfigurationManager.GetSetting("SendGridTo"))}, "[Halo 4] Authentication Failed",
						null, text));

			return false;
		}
	}
}