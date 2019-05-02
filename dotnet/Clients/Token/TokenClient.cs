using System;
using Branch.Clients.Branch;
using Branch.Packages.Contracts.ServiceToken;
using System.Threading;
using System.Threading.Tasks;
using Branch.Clients.Http.Models;
using Microsoft.Extensions.Options;

namespace Branch.Clients.Token
{
	public class TokenClient : BranchClient, IService
	{
		public TokenClient(IOptionsMonitor<BranchConfig> options) : base(options, "Token") { }
		public TokenClient(BranchConfig options) : base(options) { }

		public async Task<ResGetHalo4Token> GetHalo4Token(ReqGetHalo4Token req)
		{
			return await base.Do<ReqGetHalo4Token, ResGetHalo4Token>("1/2018-03-21/get_halo4_token", req);
		}

		public async Task<ResGetXboxLiveToken> GetXboxLiveToken(ReqGetXboxLiveToken req)
		{
			return await base.Do<ReqGetXboxLiveToken, ResGetXboxLiveToken>("1/2018-03-21/get_xboxlive_token", req);
		}
	}
}
