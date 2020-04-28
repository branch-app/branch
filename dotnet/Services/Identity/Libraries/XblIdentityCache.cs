using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Global.Contracts;
using Branch.Global.Extensions;
using Branch.Global.Libraries;
using Branch.Global.Models.XboxLive;
using Branch.Services.Token;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace Branch.Services.Identity.Libraries
{
	public class XblIdentityCache
	{
		private ILogger _logger { get; }
		private IRedisClientsManager _redisClientsManager { get; }
		private TokenClient _tokenClient { get; }
		private JsonClient _client { get; }

		private string _authHeader = "XBL3.0 x={0};{1}";
		private string _pathTemplate = "/users/{0}({1})/profile/settings";

		public XblIdentityCache(
			ILoggerFactory loggerFactory,
			IRedisClientsManager redisClientsManager,
			TokenClient tokenClient
		)
		{
			_logger = loggerFactory.CreateLogger(typeof(XblIdentityCache));
			_redisClientsManager = redisClientsManager;
			_tokenClient = tokenClient;
			_client = new JsonClient("https://profile.xboxlive.com");
		}

		public async Task<GetXblIdentityRes> GetByGamertag(string gamertag, bool useCache)
		{
			return await GetIdentity(LookupType.Gamertag, gamertag, useCache);
		}

		public async Task<GetXblIdentityRes> GetByXuid(string xuid, bool useCache)
		{
			return await GetIdentity(LookupType.Xuid, xuid, useCache);
		}

		private async Task<GetXblIdentityRes> GetIdentity(LookupType type, string value, bool useCache)
		{
			using (var client = _redisClientsManager.GetClient())
			{
				if (useCache)
				{
					var ident = client.GetJson<GetXblIdentityRes>(GenerateRedisKey(type, value));
					if (ident != null)
						return ident;
				}

				var (gamertag, xuid) = await GetProfileSettings(type, value);
				var identity = new GetXblIdentityRes
				{
					CacheInfo = new CacheInfo(DateTime.UtcNow, TimeSpan.FromMinutes(30)),
					Gamertag = gamertag,
					Xuid = xuid,
				};

				var jsonStr = JsonConvert.SerializeObject(identity);
				var expiry = (DateTime) identity.CacheInfo.ExpiresAt;

				client.Set(GenerateRedisKey(LookupType.Gamertag, gamertag.ToSlug()), jsonStr, expiry);
				client.Set(GenerateRedisKey(LookupType.Xuid, xuid), jsonStr, expiry);

				return identity;
			}
		}

		private async Task<(string gamertag, string xuid)> GetProfileSettings(LookupType type, string value)
		{
			var token = await _tokenClient.GetXblToken(null, new GetTokenRequest());
			var options = new HttpClientOptions();
			var query = new Dictionary<string, string>();
			var path = string.Format(_pathTemplate, type.ToString().ToLowerInvariant(), value);

			options.Headers.Add("authorization", string.Format(_authHeader, token.Uhs, token.Token));
			options.Headers.Add("X-XBL-Contract-Version", "2");
			query.Add("settings", "gamertag");

			var response = await _client.Do<ProfileSettings>("GET", path, query, options);
			var user = response.ProfileUsers[0];
			var xuid = user.ID.ToString();
			var gamertag = user.Settings.First(s => s.ID == "Gamertag").Value;

			return (gamertag, xuid);
		}
		
		private string GenerateRedisKey(LookupType type, string value)
		{
			return $"{type.ToString().ToLowerInvariant()}-{value.Trim().ToSlug()}";
		}

		private enum LookupType
		{
			Gamertag,
			Xuid
		}
	}
}
