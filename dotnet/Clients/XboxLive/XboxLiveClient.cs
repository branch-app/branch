using Amazon.S3;
using Branch.Clients.Cache;
using Branch.Clients.Http;
using Branch.Clients.Http.Models;
using Branch.Clients.Token;
using Branch.Models.Common.XboxLive;
using Branch.Packages.Contracts.ServiceToken;
using Branch.Packages.Enums.External.XboxLive;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Bae;
using Branch.Packages.Models.Common.XboxLive;
using Branch.Packages.Models.XboxLive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Branch.Clients.S3;

namespace Branch.Clients.XboxLive
{
	public class XboxLiveClient : CacheClient
	{
		private readonly TokenClient _tokenClient;

		protected readonly HttpClient _profileClient;

		protected string profileBaseUrl = "https://profile.xboxlive.com/users/";
		protected string profileSettingsUrl = "{0}({1})/profile/settings";

		private string authHeader = "XBL3.0 x={0};{1}";

		public XboxLiveClient(TokenClient tokenClient, S3Client s3Client) : base(s3Client)
		{
			var httpHeaders = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" },
				{ "Accept", "application/json" },
			};
			var httpOptions = new Options(httpHeaders, TimeSpan.FromSeconds(2));

			this._tokenClient = tokenClient;
			this._profileClient = new HttpClient(profileBaseUrl, httpOptions);
		}

		/// <summary>
		/// Gets the profile settings of a player. If no settings are given, it is assumed
		/// all are wanted.
		/// </summary>
		public async Task<ProfileSettings> GetProfileSettings(XboxLiveIdentityType type, string value, params ProfileSetting[] settings)
		{
			string[] strs = settings?.Any() == false
				? Enum.GetNames(typeof(ProfileSetting))
				: settings.Select(ps => Enum.GetName(typeof(ProfileSetting), ps)).ToArray();

			var path = string.Format(profileSettingsUrl, type.ToString(), value);
			var query = new Dictionary<string, string> { { "settings", string.Join(",", strs) } };

			return await requestXboxLiveData<ProfileSettings>(_profileClient, 2, path, query, null);
		}

		protected async Task<TRes> requestXboxLiveData<TRes>(HttpClient client, uint contractVersion, string path, Dictionary<string, string> query, Options newOpts = null)
			where TRes: class, new()
		{
			var auth = await getAuth();
			var opts = newOpts ?? new Options();
			opts.Headers.Add("authorization", auth);
			opts.Headers.Add("X-XBL-Contract-Version", contractVersion.ToString());

			var output = await client.Do("GET", path, query, opts);
			var str = await output.resp.Content.ReadAsStringAsync();

			if (output.resp.IsSuccessStatusCode)
			{
				if (String.IsNullOrWhiteSpace(str))
					return null;

				return JsonConvert.DeserializeObject<TRes>(str);
			}

			var isJson = output.resp.Content.Headers.ContentType.MediaType == "application/json";

			if (output.resp.StatusCode == HttpStatusCode.Unauthorized)
				throw new BaeException("xbl_auth_failure", createExceptionMeta(output));

			if (String.IsNullOrWhiteSpace(str) || !isJson)
				throw new BaeException("request_failed", createExceptionMeta(output));

			var error = JsonConvert.DeserializeObject<Error>(str);

			switch(error.Code)
			{
				case ResponseCode.ProfileNotFound:
				case ResponseCode.XUIDInvalid:
					throw new BaeException("xbl_identity_not_found");

				default:
					{
						var meta = createExceptionMeta(output);
						meta.Add("Response", error);

						throw new BaeException("request_failed", meta);
					}
			}
		}

		private async Task<string> getAuth()
		{
			var auth = await _tokenClient.GetXboxLiveToken(new ReqGetXboxLiveToken());

			return string.Format(authHeader, auth.Uhs, auth.Token);
		}

		private Dictionary<string, object> createExceptionMeta((System.Net.Http.HttpRequestMessage req, System.Net.Http.HttpResponseMessage res) output)
		{
			var (req, res) = output;

			return new Dictionary<string, object>
			{
				{ "Url", req.RequestUri.ToString() },
				{ "Verb", req.Method.Method },
				{ "StatusCode", res.StatusCode },
			};
		}
	}
}
