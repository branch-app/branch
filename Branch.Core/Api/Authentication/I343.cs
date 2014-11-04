using System;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Branch.Core.Storage;
using Branch.Models.Services._343;
using Branch.Models.Sql;
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
			using (var sqlStorage = new SqlStorage())
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

							var waypointToken = JsonConvert.DeserializeObject<Halo4Waypoint>(response.RawText);
							if (waypointToken != null && !String.IsNullOrWhiteSpace(waypointToken.SpartanToken))
							{
								var authentication = new Models.Sql.Authentication
								{
									Type = AuthenticationType.Halo4,
									IsValid = true,
									Key = waypointToken.SpartanToken
								};
								sqlStorage.Authentications.AddOrUpdate(a => a.Type, authentication);
								sqlStorage.SaveChanges();

								everythingWentGucci = true;
							}
						}
						catch (Exception ex)
						{
							Trace.TraceError(ex.ToString());
							everythingWentGucci = false;
						}
					}

					if (everythingWentGucci)
						break;
				}
				

				if (everythingWentGucci)
					return true;

				// make sure halo 4 auth row has been deleted
				var invalidAuthentication = sqlStorage.Authentications.FirstOrDefault(a => a.Type == AuthenticationType.Halo4);
				if (invalidAuthentication != null)
				{
					invalidAuthentication.Key = null;
					invalidAuthentication.IsValid = false;
				}

				// send glorious email!
				var text =
					String.Format(
						"Sup guys, {0}Halo 4's authenication failed to update. Might want to look into it. Below is the response the server recieved from the auth service: {0}{0}{1}{0}{0}Best Regards,{0}Branch",
						Environment.NewLine, strResponse);

				new Web(new NetworkCredential(CloudConfigurationManager.GetSetting("SendGridUser"),
					CloudConfigurationManager.GetSetting("SendGridPass"))).Deliver(
						new SendGridMessage(new MailAddress("info@branchapp.co"),
							new[]
							{
								new MailAddress(CloudConfigurationManager.GetSetting("SendGridTo")), 
								new MailAddress("connor.tumbleson@gmail.com"),
							},
							"[Halo 4] Authentication Failed", null, text));

				sqlStorage.SaveChanges();

				return false;
			}
		}
	}
}