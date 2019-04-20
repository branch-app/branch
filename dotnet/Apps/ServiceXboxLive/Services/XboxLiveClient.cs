using XboxLiveClientBase = Branch.Clients.XboxLive.XboxLiveClient;
using Branch.Clients.Token;
using Branch.Clients.Cache;
using Amazon.S3;

namespace Branch.Apps.ServiceXboxLive.Services
{
	public partial class XboxLiveClient : XboxLiveClientBase
	{
		private const string storageBucket = "branch-app-stats";

		public XboxLiveClient(TokenClient tokenClient, AmazonS3Client s3Client)
		{

		}

		// public XboxLiveClient(TokenClient tokenClient, IdentityClient identityClient, AmazonS3Client s3Client)
		// 	: base(storageBucket, s3Client)
		// {
		// 	this.tokenClient = tokenClient;
		// }

		// public async Task<(ServiceRecordResponse serviceRecord, ICacheInfo cacheInfo)> GetServiceRecord(Identity identity)
		// {
		// 	var path = $"players/{identity.Gamertag}/h4/servicerecord";
		// 	var key = $"halo-4/service-record/{identity.XUIDStr}.json";
		// 	var expire = TimeSpan.FromMinutes(10);
		// 	var cacheInfo = await fetchCacheInfo(key);

		// 	if (cacheInfo != null && cacheInfo.IsFresh())
		// 		return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

		// 	var response = await requestWaypointData<External.ServiceRecordResponse>(path, null, key);

		// 	if (response == null && cacheInfo != null)
		// 		return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

		// 	// Transpile and enrich content
		// 	var finalCacheInfo = new CacheInfo(DateTime.UtcNow, expire);
		// 	var final = transpiler.ServiceRecord(response);
		// 	final = await enricher.ServiceRecord(final, response);

		// 	TaskExt.FireAndForget(() => cacheContent(key, final, finalCacheInfo));

		// 	return (final, finalCacheInfo);
		// }

		// public async Task<(RecentMatchesResponse recentMatches, ICacheInfo cacheInfo)> GetRecentMatches(Identity identity, GameMode gameMode, uint startAt, uint count)
		// {
		// 	var newCount = count + 1;
		// 	var gameModeStr = gameMode.ToString("d");
		// 	var query = new Dictionary<string, string>
		// 	{
		// 		{ "gamemodeid", gameModeStr },
		// 		{ "startat", startAt.ToString() },
		// 		{ "count", newCount.ToString() },
		// 	};
		// 	var path = $"players/{identity.Gamertag}/h4/matches";
		// 	var key = $"halo-4/recent-matches/{identity.XUIDStr}-{gameModeStr}-{startAt}-{newCount}.json";
		// 	var expire = TimeSpan.FromMinutes(10);
		// 	var cacheInfo = await fetchCacheInfo(key);

		// 	if (cacheInfo != null && cacheInfo.IsFresh())
		// 		return (await fetchContent<RecentMatchesResponse>(key), cacheInfo);

		// 	var response = await requestWaypointData<External.RecentMatchesResponse>(path, query, key);

		// 	if (response == null && cacheInfo != null)
		// 		return (await fetchContent<RecentMatchesResponse>(key), cacheInfo);

		// 	// Transpile and enrich content
		// 	var final = transpiler.RecentMatches(response, newCount);
		// 	// final = await enricher.RecentMatches(final, response); // Nothing to enrich here I think

		// 	var finalCacheInfo = new CacheInfo(DateTime.UtcNow, expire);

		// 	TaskExt.FireAndForget(() => cacheContent(key, final, finalCacheInfo));

		// 	return (final, finalCacheInfo);
		// }
	}
}
