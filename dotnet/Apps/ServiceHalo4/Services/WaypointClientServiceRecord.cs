using System;
using System.Threading.Tasks;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Extensions;
using Branch.Packages.Models.Common.XboxLive;
using Branch.Packages.Models.Halo4;
using External = Branch.Packages.Models.External.Halo4;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class WaypointClient
	{
		public async Task<(ServiceRecordResponse serviceRecord, ICacheInfo cacheInfo)> GetServiceRecord(Identity identity)
		{
			var path = $"players/{identity.Gamertag}/h4/servicerecord";
			var key = $"halo-4/service-record/{identity.XUIDStr}.json";
			var expire = TimeSpan.FromMinutes(10);
			var cacheInfo = await fetchContentCacheInfo(key);

			if (cacheInfo != null && cacheInfo.IsFresh())
				return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

			var response = await requestWaypointData<External.ServiceRecordResponse>(path, null, key);

			if (response != null && cacheInfo != null)
				return (await fetchContent<ServiceRecordResponse>(key), cacheInfo);

			var final = await transpileServiceRecord(response);
			var finalCacheInfo = new CacheInfo(DateTime.UtcNow, expire);

			// TODO(0xdeafcafe): Don't forget to un-comment!
			// TaskExt.FireAndForget(() => cacheContent(key, final, finalCacheInfo));

			return (final, finalCacheInfo);
		}

		private async Task<ServiceRecordResponse> transpileServiceRecord(External.ServiceRecordResponse src)
		{
			var output = new ServiceRecordResponse
			{
				DateFidelity = 1,
				FirstPlayed = src.FirstPlayedUtc,
				LastPlayed = src.LastPlayedUtc,
				XP = src.XP,
				SpartanPoints = src.SpartanPoints,
				TotalGamesStarted = src.TotalGamesStarted,
				TotalMedalsEarned = src.TotalMedalsEarned,
				TotalGameplay = src.TotalGameplay,
				TotalChallengesCompleted = src.TotalChallengesCompleted,
				TotalLoadoutItemsPurchased = src.TotalLoadoutItemsPurchased,
				TotalCommendationProgress = src.TotalCommendationProgress,
			};

			var identity = await identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = XboxLiveIdentityType.Gamertag,
				Value = src.Gamertag,
			});

			output.Identity = new Packages.Models.Halo4.ServiceRecord.Identity
			{
				XUID = identity.XUID,
				ServiceTag = src.ServiceTag,
				Emblem = new Packages.Models.Halo4.ServiceRecord.Emblem
				{
					EmblemUrl = src.EmblemImageUrl.AssetUrl,
					BackgroundUrl = src.EmblemImageUrl.AssetUrl,
				},
			};
			output.FavoriteWeapon = new Packages.Models.Halo4.ServiceRecord.FavoriteWeapon
			{
				ID = src.FavoriteWeaponId,
				Name = src.FavoriteWeaponName,
				Description = src.FavoriteWeaponDescription,
				ImageUrl = src.FavoriteWeaponImageUrl.AssetUrl,
				TotalKills = src.FavoriteWeaponTotalKills,
			};

			return output;
		}
	}
}
