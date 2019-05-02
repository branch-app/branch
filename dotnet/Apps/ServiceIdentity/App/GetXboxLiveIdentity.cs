using System.Threading.Tasks;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Contracts.Common.Branch;

namespace Branch.Apps.ServiceIdentity.App
{
	public partial class Application
	{
		public async Task<ResGetXboxLiveIdentity> GetXboxLiveIdentity(XboxLiveIdentityType type, string value)
		{
			var identity = await identityMapper.GetIdentity(type, value);

			return new ResGetXboxLiveIdentity
			{
				CacheInfo = new CacheInfo(identity),
				Gamertag = identity.Gamertag,
				XUID = identity.XUID,
			};
		}
	}
}
