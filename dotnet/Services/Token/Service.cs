using System.Threading.Tasks;
using Branch.Global.Contracts;
using Microsoft.AspNetCore.Http;

namespace Branch.Services.Token
{
	public interface ITokenService
	{
		Task<GetXblTokenResponse> GetXblToken(HttpContext ctx, GetTokenRequest req);
	}

	public class GetTokenRequest
	{
		public bool IgnoreCache { get; set; }
	}

	public class GetXblTokenResponse : IBranchResponse
	{
		public CacheInfo CacheInfo { get; set; }

		public string Token { get; set; }

		public string Uhs { get; set; }
	}
}
