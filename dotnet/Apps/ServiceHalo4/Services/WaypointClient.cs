using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Branch.Apps.ServiceHalo4.Enums.Waypoint;
using Branch.Clients.Cache;
using Branch.Clients.Token;
using Branch.Clients.Identity;
using Branch.Clients.Json;
using Branch.Clients.Http.Models;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceToken;
using Branch.Packages.Crypto;
using Branch.Packages.Enums.Halo4;
using Branch.Packages.Enums.ServiceIdentity;
using Crpc.Exceptions;
using Branch.Packages.Extensions;
using Branch.Packages.Models.Common.XboxLive;
using Branch.Packages.Models.Halo4;
using Branch.Packages.Models.XboxLive;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using External = Branch.Apps.ServiceHalo4.Models.Waypoint;
using Branch.Clients.S3;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class WaypointClient : CacheClient
	{
		private readonly TokenClient _tokenClient;
		private readonly JsonClient _presenceClient;
		private readonly JsonClient _statsClient;
		private readonly JsonClient _settingsClient;
		private readonly JsonClient _optionsClient;
		private readonly Transpiler _transpiler;
		private readonly Enricher _enricher;

		private const string presenceUrl = "https://presence.svc.halowaypoint.com/en-US/";
		private const string statsUrl = "https://stats.svc.halowaypoint.com/en-US/";
		private const string settingsUrl = "https://settings.svc.halowaypoint.com/";
		private const string optionsUrl = "https://settings.svc.halowaypoint.com/RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752";
		private const string authHeader = "X-343-Authorization-Spartan";

		public WaypointClient(TokenClient tokenClient, IdentityClient identityClient, S3Client s3Client)
			: base(s3Client)
		{
			this._tokenClient = tokenClient;
			this._transpiler = new Transpiler();
			this._enricher = new Enricher(identityClient);
			this._presenceClient = new JsonClient(presenceUrl);
			this._statsClient = new JsonClient(statsUrl);
			this._settingsClient = new JsonClient(settingsUrl);
			this._optionsClient = new JsonClient(optionsUrl);
		}

		private async Task<T> requestWaypointData<T>(string path, Dictionary<string, string> query, string bucketKey)
			where T : External.WaypointResponse
		{
			var auth = await getTokenHeaders();
			var opts = new Options(auth);

			// TODO(0xdeafcafe): Handle waypoint errors
			var response = await _statsClient.Do<T>("GET", path, query, opts);

			switch (response.StatusCode)
			{
				// Should never happen on player-related requests
				case ResponseCode.NotFound:
					throw new CrpcException(
						"content_not_found",
						new Dictionary<string, object> {
							{ "Path", path },
							{ "Query", query },
						}
					);

				case ResponseCode.PlayerHasNotPlayedHalo4:
					throw new CrpcException("player_never_played");

				case ResponseCode.Okay:
				case ResponseCode.Found:
				default:
					return response;
			}
		}

		private async Task<Dictionary<string, string>> getTokenHeaders()
		{
			var resp = await _tokenClient.GetHalo4Token(new ReqGetHalo4Token());

			return new Dictionary<string, string> {{ "X-343-Tokenorization-Spartan", resp.SpartanToken }};
		}

		public async Task<(ServiceRecordResponse serviceRecord, ICacheInfo cacheInfo)> GetServiceRecord(Identity identity)
		{
			var path = $"players/{identity.Gamertag}/h4/servicerecord";
			var key = $"halo-4/service-record/{identity.XUIDStr}.json";
			var expire = TimeSpan.FromMinutes(10);
			var cacheInfo = await fetchCacheInfo(key);

			if (cacheInfo != null && cacheInfo.IsFresh())
				return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

			var response = await requestWaypointData<External.ServiceRecordResponse>(path, null, key);

			if (response == null && cacheInfo != null)
				return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

			// Transpile and enrich content
			var finalCacheInfo = new CacheInfo(DateTime.UtcNow, expire);
			var final = _transpiler.ServiceRecord(response);
			final = await _enricher.ServiceRecord(final, response);

			TaskExt.FireAndForget(() => cacheContent(key, final, finalCacheInfo));

			return (final, finalCacheInfo);
		}

		public async Task<(RecentMatchesResponse recentMatches, ICacheInfo cacheInfo)> GetRecentMatches(Identity identity, GameMode gameMode, uint startAt, uint count)
		{
			var newCount = count + 1;
			var gameModeStr = gameMode.ToString("d");
			var query = new Dictionary<string, string>
			{
				{ "gamemodeid", gameModeStr },
				{ "startat", startAt.ToString() },
				{ "count", newCount.ToString() },
			};
			var path = $"players/{identity.Gamertag}/h4/matches";
			var key = $"halo-4/recent-matches/{identity.XUIDStr}-{gameModeStr}-{startAt}-{newCount}.json";
			var expire = TimeSpan.FromMinutes(10);
			var cacheInfo = await fetchCacheInfo(key);

			if (cacheInfo != null && cacheInfo.IsFresh())
				return (await fetchContent<RecentMatchesResponse>(key), cacheInfo);

			var response = await requestWaypointData<External.RecentMatchesResponse>(path, query, key);

			if (response == null && cacheInfo != null)
				return (await fetchContent<RecentMatchesResponse>(key), cacheInfo);

			// Transpile and enrich content
			var final = _transpiler.RecentMatches(response, newCount);
			// final = await enricher.RecentMatches(final, response); // Nothing to enrich here I think

			var finalCacheInfo = new CacheInfo(DateTime.UtcNow, expire);

			TaskExt.FireAndForget(() => cacheContent(key, final, finalCacheInfo));

			return (final, finalCacheInfo);
		}
	}
}
