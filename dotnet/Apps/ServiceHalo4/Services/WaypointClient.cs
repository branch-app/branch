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
using Branch.Packages.Exceptions;
using Branch.Packages.Extensions;
using Branch.Packages.Models.Common.XboxLive;
using Branch.Packages.Models.Halo4;
using Branch.Packages.Models.XboxLive;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using External = Branch.Apps.ServiceHalo4.Models.Waypoint;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class WaypointClient : CacheClient
	{
		private TokenClient tokenClient { get; }
		private JsonClient presenceClient { get; }
		private JsonClient statsClient { get; }
		private JsonClient settingsClient { get; }
		private JsonClient optionsClient { get; }
		private Transpiler transpiler { get; }
		private Enricher enricher { get; }

		private const string presenceUrl = "https://presence.svc.halowaypoint.com/en-US/";
		private const string statsUrl = "https://stats.svc.halowaypoint.com/en-US/";
		private const string settingsUrl = "https://settings.svc.halowaypoint.com/";
		private const string optionsUrl = "https://settings.svc.halowaypoint.com/RegisterClientService.svc/register/webapp/AE5D20DCFA0347B1BCE0A5253D116752";
		private const string authHeader = "X-343-Authorization-Spartan";
		private const string storageBucket = "branch-app-stats";

		public WaypointClient(TokenClient tokenClient, IdentityClient identityClient, AmazonS3Client s3Client)
			: base(storageBucket, s3Client)
		{
			this.tokenClient = tokenClient;
			this.transpiler = new Transpiler();
			this.enricher = new Enricher(identityClient);
			this.presenceClient = new JsonClient(presenceUrl);
			this.statsClient = new JsonClient(statsUrl);
			this.settingsClient = new JsonClient(settingsUrl);
			this.optionsClient = new JsonClient(optionsUrl);
		}

		private async Task<T> requestWaypointData<T>(string path, Dictionary<string, string> query, string bucketKey)
			where T : External.WaypointResponse
		{
			var auth = await getTokenHeaders();
			var opts = new Options(auth);

			// TODO(0xdeafcafe): Handle waypoint errors
			var response = await statsClient.Do<T, Exception>("GET", path, query, opts);

			switch (response.StatusCode)
			{
				// Should never happen on player-related requests
				case ResponseCode.NotFound:
					throw new BranchException(
						"content_not_found",
						new Dictionary<string, object> {
							{ "Path", path },
							{ "Query", query },
						}
					);

				case ResponseCode.PlayerHasNotPlayedHalo4:
					throw new BranchException("player_never_played");

				case ResponseCode.Okay:
				case ResponseCode.Found:
				default:
					return response;
			}
		}

		private async Task<Dictionary<string, string>> getTokenHeaders()
		{
			var resp = await tokenClient.GetHalo4Token(new ReqGetHalo4Token());

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
			var final = transpiler.ServiceRecord(response);
			final = await enricher.ServiceRecord(final, response);

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
			var final = transpiler.RecentMatches(response, newCount);
			// final = await enricher.RecentMatches(final, response); // Nothing to enrich here I think

			var finalCacheInfo = new CacheInfo(DateTime.UtcNow, expire);

			TaskExt.FireAndForget(() => cacheContent(key, final, finalCacheInfo));

			return (final, finalCacheInfo);
		}
	}
}
