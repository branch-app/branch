using System.Threading.Tasks;
using Branch.Packages.Contracts.ServiceHalo4;
using Branch.Packages.Contracts.Common.Branch;
using Branch.Packages.Contracts.ServiceIdentity;
using AutoMapper;

namespace Branch.Apps.ServiceHalo4.App
{
	public partial class Application
	{
		public async Task<ResGetPlayerOverview> GetPlayerOverview(IdentityRequest identReq)
		{
			var identity = await _identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = identReq.Type,
				Value = identReq.Value,
			});

			var (resp, cacheInfo) = await _waypointClient.GetServiceRecord(identity.ToIdentity());
			var result = Mapper.Map<ResGetPlayerOverview>(resp);
			result.CacheInfo = cacheInfo;

			return result;
		}
	}
}
