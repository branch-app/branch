using System;
using Branch.Clients.Branch;
using System.Threading;
using System.Threading.Tasks;
using Branch.Clients.Json;
using Branch.Packages.Contracts.ServiceIdentity;
using Microsoft.Extensions.Options;

namespace Branch.Clients.Identity
{
	public class IdentityClient : BranchClient, IService
	{
		public IdentityClient(IOptionsMonitor<BranchConfig> options) : base(options, "Identity") { }

		public async Task<ResGetXboxLiveIdentity> GetXboxLiveIdentity(ReqGetXboxLiveIdentity req)
		{
			return await base.Do<ReqGetXboxLiveIdentity, ResGetXboxLiveIdentity>("1/2018-08-19/get_xboxlive_identity", req);
		}
	}
}
