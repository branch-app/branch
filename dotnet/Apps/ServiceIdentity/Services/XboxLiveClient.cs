using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Branch.Apps.ServiceIdentity.Models;
using Branch.Clients.Token;
using Branch.Clients.Http;
using Branch.Clients.Http.Models;
using Branch.Packages.Contracts.ServiceToken;
using Branch.Packages.Enums.External.XboxLive;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Exceptions;
using Branch.Packages.Models.XboxLive;
using Newtonsoft.Json;

namespace Branch.Apps.ServiceIdentity.Services
{
	public class XboxLiveClient
	{
		private TokenClient tokenClient { get; }
		private HttpClient httpClient { get; }

		private string baseUrl = "https://profile.xboxlive.com/users/";
		private string profileSettingsUrl = "{0}({1})/profile/settings";
		private string authHeader = "XBL3.0 x={0};{1}";

		public XboxLiveClient(TokenClient tokenClient)
		{
			var httpHeaders = new Dictionary<string, string>
			{
				{ "X-XBL-Contract-Version", "2" },
				{ "Content-Type", "application/json" },
				{ "Accept", "application/json" },
			};
			var httpOptions = new Options(httpHeaders, TimeSpan.FromSeconds(2));

			this.tokenClient = tokenClient;
			this.httpClient = new HttpClient(baseUrl, httpOptions);
		}

		public async Task<ProfileSettings> GetProfileSettings(XboxLiveIdentityType type, string value)
		{
			var path = string.Format(profileSettingsUrl, type.ToString(), value);
			var query = new Dictionary<string, string> { { "settings", "gamertag"} };

			return await requestXboxLiveData<ProfileSettings>(path, query, null);
		}

		private async Task<TRes> requestXboxLiveData<TRes>(string path, Dictionary<string, string> query, Options newOpts = null)
			where TRes: class, new()
		{
			var auth = await getAuth();
			var opts = newOpts ?? new Options();
			opts.Headers.Add("authorization", auth);

			var output = await httpClient.Do("GET", path, query, opts);
			var str = await output.resp.Content.ReadAsStringAsync();

			if (output.resp.IsSuccessStatusCode)
			{
				if (String.IsNullOrWhiteSpace(str))
					return null;

				return JsonConvert.DeserializeObject<TRes>(str);
			}

			var isJson = output.resp.Content.Headers.ContentType.MediaType == "application/json";

			if (output.resp.StatusCode == HttpStatusCode.Unauthorized)
				throw new BranchException("xbl_auth_failure", createExceptionMeta(output));

			if (String.IsNullOrWhiteSpace(str) || !isJson)
				throw new BranchException("request_failed", createExceptionMeta(output));

			var error = JsonConvert.DeserializeObject<XboxLiveError>(str);

			switch(error.Code)
			{
				case ResponseCode.ProfileNotFound:
				case ResponseCode.XUIDInvalid:
					throw new BranchException("xbl_identity_not_found");

				default:
					{
						var meta = createExceptionMeta(output);
						meta.Add("Response", error);

						throw new BranchException("request_failed", meta);
					}
			}
		}

		private async Task<string> getAuth()
		{
			var auth = await tokenClient.GetXboxLiveToken(new ReqGetXboxLiveToken());

			return string.Format(authHeader, auth.Uhs, auth.Token);
		}

		private Dictionary<string, object> createExceptionMeta((System.Net.Http.HttpRequestMessage req, System.Net.Http.HttpResponseMessage resp) output)
		{

			return new Dictionary<string, object>
			{
				{ "url", output.req.RequestUri.ToString() },
				{ "verb", output.req.Method.Method },
				{ "status_code", output.resp.StatusCode },
			};
		}
	}
}
