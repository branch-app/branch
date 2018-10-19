using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Branch.Clients.Auth;
using Branch.Clients.Json;
using Branch.Clients.Json.Models;
using Branch.Packages.Contracts.ServiceAuth;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Models.XboxLive;

namespace Branch.Apps.ServiceIdentity.Services
{
	public class XboxLiveClient
	{
		private AuthClient authClient { get; }
		private JsonClient jsonClient { get; }

		private string baseUrl = "https://profile.xboxlive.com/users/";
		private string profileSettingsUrl = "{0}({1})/profile/settings";
		private string authHeader = "XBL3.0 x={0};{1}";

		public XboxLiveClient(AuthClient authClient)
		{
			var jsonOptions = new Options(new Dictionary<string, string> { { "x-xbl-contract-version", "2" } });

			this.authClient = authClient;
			this.jsonClient = new JsonClient(baseUrl, jsonOptions);
		}

		public async Task<ProfileSettings> GetProfileSettings(XboxLiveIdentityType type, string value)
		{
			var auth = await getAuth();
			var populatedPath = string.Format(profileSettingsUrl, type.ToString(), value);
			var query = new Dictionary<string, string> { { "settings", "gamertag"} };
			var headers = new Dictionary<string, string> { { "authorization", string.Format(authHeader, auth.Uhs, auth.Token) } };
			var options = new Options(headers);

			return await jsonClient.Do<ProfileSettings, object>("GET", populatedPath, query, options);
		}

		private async Task<ResGetXboxLiveToken> getAuth()
		{
			return await authClient.GetXboxLiveToken(new ReqGetXboxLiveToken());
		}
	}
}
