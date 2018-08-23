using System;
using Branch.Clients.Branch;
using Branch.Packages.Contracts.ServiceAuth;
using System.Threading;
using System.Threading.Tasks;

namespace Branch.Clients.Auth
{
	public class AuthClient : BranchClient, IService
	{
		public AuthClient(string baseUrl, string key)
			: base(baseUrl, key) { }

		public async Task<ResGetHalo4Token> GetHalo4Token()
		{
			return await base.Do<ResGetHalo4Token>("1/2018-03-21/get_halo4_token");
		}

		public async Task<ResGetXboxLiveToken> GetXboxLiveToken()
		{
			return await base.Do<ResGetXboxLiveToken>("1/2018-03-21/get_xboxlive_token");
		}
	}
}
