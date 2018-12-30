using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Clients.Identity;
using Branch.Packages.Contracts.ServiceIdentity;
using Branch.Packages.Enums.ServiceIdentity;
using Branch.Packages.Models.Halo4;
using ExtSR = Branch.Apps.ServiceHalo4.Models.Waypoint;

namespace Branch.Apps.ServiceHalo4.Services
{
	public partial class Enricher
	{
		private IdentityClient identityClient { get; }

		public Enricher(IdentityClient identity)
		{
			identityClient = identity;

		}

		public async Task<ServiceRecordResponse> ServiceRecord(ServiceRecordResponse sr, ExtSR.ServiceRecordResponse ext)
		{
			var identity = await identityClient.GetXboxLiveIdentity(new ReqGetXboxLiveIdentity
			{
				Type = XboxLiveIdentityType.Gamertag,
				Value = ext.Gamertag,
			});

			sr.Identity.XUID = identity.XUID;
			sr.Identity.Gamertag = identity.Gamertag;

			return sr;
		}
	}
}
