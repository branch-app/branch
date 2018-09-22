using System;
using Branch.Clients.Branch;
using System.Threading;
using System.Threading.Tasks;
using Branch.Clients.Json.Models;
using Branch.Packages.Contracts.ServiceIdentity;

namespace Branch.Clients.Identity
{
	public class IdentityClient : BranchClient, IService
	{
		public IdentityClient(string baseUrl, string key)
			: base(baseUrl, key)
		{ }

		public async Task<ResGetXboxLiveIdentity> GetXboxLiveIdentity(ReqGetXboxLiveIdentity req)
		{
			return await base.Do<ReqGetXboxLiveIdentity, ResGetXboxLiveIdentity>("1/2018-08-19/get_xboxlive_identity", req);
		}
	}
}
