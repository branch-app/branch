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
using Branch.Clients.Auth;
using Branch.Clients.Identity;
using Branch.Clients.Json;
using Branch.Clients.Http.Models;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceAuth;
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
	public partial class WaypointClient
	{
		private AuthClient authClient { get; }
		private AmazonS3Client s3Client { get; }
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

		public WaypointClient(AuthClient authClient, IdentityClient identityClient, AmazonS3Client s3Client)
		{
			this.authClient = authClient;
			this.s3Client = s3Client;
			this.transpiler = new Transpiler();
			this.enricher = new Enricher(identityClient);
			this.presenceClient = new JsonClient(presenceUrl);
			this.statsClient = new JsonClient(statsUrl);
			this.settingsClient = new JsonClient(settingsUrl);
			this.optionsClient = new JsonClient(optionsUrl);
		}

		private async Task<ICacheInfo> fetchContentCacheInfo(string bucketKey)
		{
			try
			{
				var resp = await s3Client.GetObjectMetadataAsync(storageBucket, bucketKey);
				// var cacheHash = resp.Metadata["x-amz-meta-content-hash"];
				var contentCreation = DateTime.Parse(resp.Metadata["x-amz-meta-content-creation"]);
				var contentExpiration = DateTime.Parse(resp.Metadata["x-amz-meta-content-expiration"]);

				return new CacheInfo(contentCreation, contentExpiration);
			}
			catch (AmazonS3Exception ex) when (ex.ErrorCode == "NotFound")
			{
				return null;
			}
		}

		private async Task<T> requestWaypointData<T>(string path, Dictionary<string, string> query, string bucketKey)
			where T : External.WaypointResponse
		{
			var auth = await getAuthHeaders();
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

		private async Task<T> fetchContent<T>(string bucketKey)
		{
			try
			{
				var response = await s3Client.GetObjectAsync(storageBucket, bucketKey);

				using (var ms = new MemoryStream())
				using (var sr = new StreamReader(ms))
				using (var jr = new JsonTextReader(sr))
				{
					await response.ResponseStream.CopyToAsync(ms);
					ms.Seek(0, SeekOrigin.Begin);

					var serializer = new JsonSerializer
					{
						ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
					};

					return serializer.Deserialize<T>(jr);
				}
			}
			catch (AmazonS3Exception ex)
			{
				if (ex.ErrorCode != "NotFound")
					throw;

				throw
					new BranchException(
						"cache_not_found",
						new Dictionary<string, object> { { "key", bucketKey } }
					);
			}
		}

		private async Task cacheContent<T>(string key, T content, ICacheInfo cacheInfo)
		{
			using (var ms = new MemoryStream())
			using (var sw = new StreamWriter(ms))
			using (var jw = new JsonTextWriter(sw))
			{
				var serializer = new JsonSerializer();
				var now = DateTime.UtcNow;

				serializer.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
				serializer.Serialize(jw, content);
				await sw.FlushAsync();

				var putReq = new PutObjectRequest
				{
					BucketName = storageBucket,
					Key = key,
					ContentType = "application/json",

					// We use the StreamWriter BaseStream, as apparently using a
					// MemoryStream breaks this part and only uploads an initial partial
					// segment of the data? Fuck knows
					InputStream = sw.BaseStream,
				};

				putReq.Metadata["content-expiration"] = cacheInfo.ExpiresAt.ToISOString();
				putReq.Metadata["content-creation"] = cacheInfo.CachedAt.ToISOString();
				putReq.Metadata["content-hash"] = Sha256.HashContent(ms).ToHexString();

				await s3Client.PutObjectAsync(putReq);
			}
		}

		private async Task<Dictionary<string, string>> getAuthHeaders()
		{
			var resp = await authClient.GetHalo4Token(new ReqGetHalo4Token());

			return new Dictionary<string, string> {{ "X-343-Authorization-Spartan", resp.SpartanToken }};
		}

		public async Task<(ServiceRecordResponse serviceRecord, ICacheInfo cacheInfo)> GetServiceRecord(Identity identity)
		{
			var path = $"players/{identity.Gamertag}/h4/servicerecord";
			var key = $"halo-4/service-record/{identity.XUIDStr}.json";
			var expire = TimeSpan.FromMinutes(10);
			var cacheInfo = await fetchContentCacheInfo(key);

			if (cacheInfo != null && cacheInfo.IsFresh())
				return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

			var response = await requestWaypointData<External.ServiceRecordResponse>(path, null, key);

			if (response == null && cacheInfo != null)
				return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

			// Transpile and enrich content
			var final = transpiler.ServiceRecord(response);
			final = await enricher.ServiceRecord(final, response);

			var finalCacheInfo = new CacheInfo(DateTime.UtcNow, expire);

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
			var cacheInfo = await fetchContentCacheInfo(key);

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
