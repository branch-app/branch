using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceIdentity;
using AutoMapper;
using Branch.Packages.Enums.Halo4;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		public async Task<ResGetRecentMatches> GetRecentMatches(IdentityRequest identReq, GameMode gameMode, uint startAt, uint count)
		{
			var identity = await _identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = identReq.Type,
				Value = identReq.Value,
			});

			var response = await _waypointClient.GetRecentMatches(identity.ToIdentity(), gameMode, startAt, count);
			var result = Mapper.Map<ResGetRecentMatches>(response.recentMatches);
			result.CacheInfo = response.cacheInfo;

			return result;
		}
	}
}
